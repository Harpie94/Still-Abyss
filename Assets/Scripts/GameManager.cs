using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("UI Management")]
    [SerializeField] private Canvas tabletCanvas;

    [Header("Damage Management")]
    [SerializeField] private string repairableObjectTag = "AllRepairableObject";
    [SerializeField] private float damageInterval = 10f; // Intervalle en secondes entre les dégâts
    [SerializeField] private int damageAmount = 20; // Quantité de dégâts à infliger
    
    [Header("Gestion des salles")]
    [SerializeField] private List<Rooms> rooms = new List<Rooms>();
    private readonly Dictionary<Rooms, List<RepairableObject>> repairablesByRoom = new Dictionary<Rooms, List<RepairableObject>>();
    
    [Header("Defences Management")]
    [SerializeField] private DefenseManager defenseManager;

    public static GameManager Instance;

    public enum GameState
    {
        InGame,
        Paused,
        Dead
    }

    public GameObject player;
    public string playerTag = "Player";

    // Listes pour la gestion des objets réparables
    private List<RepairableObject> allRepairableObjects = new List<RepairableObject>();
    private List<RepairableObject> destroyedObjects = new List<RepairableObject>();

    private Coroutine damageCoroutine;

    #endregion

    #region Basic Functions

    public GameState CurrentState { get; private set; } = GameState.InGame;
    public bool IsTabletOpen { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeRooms();
        InitializeRepairableObjects();
        StartDamageSystem();
    }

    #endregion

    #region Damage Management

    private void InitializeRepairableObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(repairableObjectTag);

        allRepairableObjects.Clear();
        destroyedObjects.Clear();

        foreach (GameObject obj in objects)
        {
            RepairableObject repairableComponent = obj.GetComponent<RepairableObject>();
            if (repairableComponent != null)
            {
                allRepairableObjects.Add(repairableComponent);
            }
        }

        Debug.Log($"Trouvé {allRepairableObjects.Count} objets réparables");
        
        RebuildRepairablesByRoom();
    }

    private void StartDamageSystem()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(DamageRoutine());
    }

    private void StopDamageSystem()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamageRoutine()
    {
        while (CurrentState == GameState.InGame)
        {
            yield return new WaitForSeconds(damageInterval);

            if (CurrentState == GameState.InGame)
            {
                ApplyRandomDamage();
            }
        }
    }

    private void ApplyRandomDamage()
    {
        // Obtenir les objets qui ne sont pas encore détruits
        List<RepairableObject> availableTargets = allRepairableObjects
            .Where(obj => obj != null && !obj.IsDestroyed)
            .ToList();

        if (availableTargets.Count == 0)
        {
            Debug.Log("Tous les objets réparables sont détruits !");
            return;
        }

        // Sélectionner un objet aléatoire
        int randomIndex = Random.Range(0, availableTargets.Count);
        RepairableObject target = availableTargets[randomIndex];

        // Appliquer les dégâts
        target.TakeDamage(damageAmount);
        
        if(target.AssignedRoom != null)
            UpdateRoomHealth(target.AssignedRoom);

        // Mettre à jour la liste des objets détruits
        if (target.IsDestroyed && !destroyedObjects.Contains(target))
        {
            destroyedObjects.Add(target);
            Debug.Log($"Objet {target.name} ajouté à la liste des objets détruits");
        }

        Debug.Log($"Dégâts appliqués à {target.name} ({damageAmount} points)");
    }

    public void RefreshRepairableObjects()
    {
        InitializeRepairableObjects();
    }

    public int GetActiveRepairableObjectsCount()
    {
        return allRepairableObjects.Count(obj => obj != null && !obj.IsDestroyed);
    }

    public int GetDestroyedObjectsCount()
    {
        return destroyedObjects.Count;
    }

    #endregion

    #region UI Management

    public void ToggleTablet()
    {
        IsTabletOpen = !IsTabletOpen;

        if (IsTabletOpen)
        {
            ShowTablet();
        }
        else
        {
            HideTablet();
        }
    }

    private void ShowTablet()
    {
        if (tabletCanvas != null)
            tabletCanvas.enabled = true;

        SetCursorLockedState(false); // Unlock cursor when showing tablet

        // Deactivate player controls
        if (player != null)
        {
            var movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.SetControlsEnabledUsingTablet(false);
        }
    }

    private void HideTablet()
    {
        if (tabletCanvas != null)
            tabletCanvas.enabled = false;

        SetCursorLockedState(true); // Lock cursor when hiding tablet

        // Reactivate player controls
        if (player != null)
        {
            var movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.SetControlsEnabledUsingTablet(true);
        }
    }

    #endregion

    #region Game State Management

    public delegate void GameStateChangedHandler(GameState newState);
    public event GameStateChangedHandler OnGameStateChanged;

    public void SetGameState(GameState newState)
    {
        if (CurrentState != newState)
        {
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);

            // Gérer le système de dégâts selon l'état du jeu
            if (newState == GameState.InGame)
            {
                StartDamageSystem();
            }
            else
            {
                StopDamageSystem();
            }
        }
    }

    private void HandleGameStateChange(GameState newState)
    {
        if (player != null)
        {
            var controller = player.GetComponent<CharacterController>();
            var movement = player.GetComponent<PlayerMovement>();
            switch (newState)
            {
                case GameState.Paused:
                    ApplyPause(controller, movement);
                    break;
                case GameState.Dead:
                    ApplyDead(controller, movement);
                    break;
                case GameState.InGame:
                    ApplyResume(controller, movement);
                    break;
            }
        }
        Debug.Log($"État du jeu : {newState}");
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Paused)
        {
            SetGameState(GameState.InGame);
        }
        else if (CurrentState == GameState.InGame)
        {
            SetGameState(GameState.Paused);
        }
    }

    private void ApplyPause(CharacterController controller, PlayerMovement movement)
    {
        if (controller != null) controller.enabled = false;
        Time.timeScale = 0f;
        SetCursorLockedState(false); // Unlock cursor when paused
        // TODO: AJOUTER LE MENU PAUSE
    }

    private void ApplyDead(CharacterController controller, PlayerMovement movement)
    {
        if (controller != null) controller.enabled = false;
        if (movement != null) movement.enabled = false; // Disable player controls
        Time.timeScale = 0f;
        SetCursorLockedState(false); // Unlock cursor when dead
        // TODO: AJOUTER L'ECRAN DE MORT
    }

    private void ApplyResume(CharacterController controller, PlayerMovement movement)
    {
        if (controller != null) controller.enabled = true;
        Time.timeScale = 1f;
        SetCursorLockedState(true); // Lock cursor when resuming
    }

    void OnEnable()
    {
        OnGameStateChanged += HandleGameStateChange;
        Rooms.OnRoomAsTakenDamage += UpdateRoomDamageUI;
    }

    void OnDisable()
    {
        OnGameStateChanged -= HandleGameStateChange;
        Rooms.OnRoomAsTakenDamage -= UpdateRoomDamageUI;
    }

    public void SetCursorLockedState(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    #endregion

    #region Player Management

    public bool IsPlayer(GameObject obj)
    {
        if (player != null)
            return obj == player;
        return obj.CompareTag(playerTag);
    }

    #endregion
    
    #region Room Management
    
    private void InitializeRooms()
    {
        // Si aucune salle n’est assignée dans l’inspecteur, on récupère celles de la scène.
        if (rooms == null || rooms.Count == 0)
            rooms = FindObjectsOfType<Rooms>().ToList();
    }

    public void UpdateRoomDamageUI(Rooms room)
    {
        if (room == null) return;
        
        int damageTier = room.GetCurrentDamageTier();
        // Mettre à jour l'interface utilisateur pour la salle endommagée selon le "damage tier"
        switch (damageTier)
        {
            case 0:
                // UI pour état intact
                defenseManager.MinimapModifier(room.RoomsID, damageTier);
                Debug.Log($"Salle '{room.RoomName}' est intacte.");
                break;
            case 1:
                // UI pour dégâts mineurs
                defenseManager.MinimapModifier(room.RoomsID, damageTier);
                Debug.Log($"Salle '{room.RoomName}' a des dégâts mineurs.");
                break;
            case 2:
                // UI pour dégâts modérés
                defenseManager.MinimapModifier(room.RoomsID, damageTier);
                Debug.Log($"Salle '{room.RoomName}' a des dégâts modérés.");
                break;
            case 3:
                // UI pour dégâts sévères
                defenseManager.MinimapModifier(room.RoomsID, damageTier);
                Debug.Log($"Salle '{room.RoomName}' a des dégâts sévères.");
                break;
            case 4:
                // UI pour état détruit
                defenseManager.MinimapModifier(room.RoomsID, damageTier);
                Debug.Log($"Salle '{room.RoomName}' est détruite.");
                break;
            default:
                Debug.Log($"Salle '{room.RoomName}' a un état de dommage inconnu.");
                break;
        }
        
    }

    private void RebuildRepairablesByRoom()
    {
        repairablesByRoom.Clear();

        foreach (var ro in allRepairableObjects)
        {
            if (ro == null) continue;
            var room = ro.AssignedRoom;
            if (room == null) continue;

            if (!repairablesByRoom.TryGetValue(room, out var list))
            {
                list = new List<RepairableObject>();
                repairablesByRoom[room] = list;
            }
            list.Add(ro);
        }
    
        // Initialiser les points de vie des salles
        UpdateRoomsHealthPoints();
    }
    
    private void UpdateRoomsHealthPoints()
    {
        foreach (var room in rooms)
        {
            if (room == null) continue;
        
            // Calculer les points de vie max basés sur les objets réparables
            int totalMaxHealth = CalculateRoomMaxHealth(room);
            int currentHealth = CalculateRoomCurrentHealth(room);
        
            // Initialiser les points de vie de la salle
            room.InitializeHealth(totalMaxHealth);
            room.UpdateCurrentHealth(currentHealth);
        
            Debug.Log($"Salle '{room.RoomName}': PV max = {totalMaxHealth}, PV actuels = {currentHealth}");
        }
    }
    
    private int CalculateRoomMaxHealth(Rooms room)
    {
        if (room == null || !repairablesByRoom.TryGetValue(room, out var repairables))
            return 0;
        
        int totalMaxHealth = 0;
        foreach (var obj in repairables)
        {
            if (obj != null)
                totalMaxHealth += obj.GetMaxHealth();
        }
    
        return totalMaxHealth;
    }
    
    private int CalculateRoomCurrentHealth(Rooms room)
    {
        if (room == null || !repairablesByRoom.TryGetValue(room, out var repairables))
        {
            Debug.Log($"Aucun objet réparables trouvé pour la salle '{room?.RoomName ?? "null"}'");
            return 0;
        }
        
        int totalCurrentHealth = 0;
        foreach (var obj in repairables)
        {
            if (obj != null)
                totalCurrentHealth += obj.GetCurrentHealth();
        }
    
        return totalCurrentHealth;
    }
    
    public int GetRoomMaxHealth(Rooms room)
    {
        return room != null ? room.MaxHealthPoints : 0;
    }
    
    public int GetRoomCurrentHealth(Rooms room)
    {
        return room != null ? room.CurrentHealthPoints : 0;
    }
    
    public float GetRoomHealthPercentage(Rooms room)
    {
        return room != null ? room.HealthPercentage : 0f;
    }
    
    public void UpdateRoomHealth(Rooms room)
    {
        if (room == null) return;
    
        int currentHealth = CalculateRoomCurrentHealth(room);
        room.UpdateCurrentHealth(currentHealth);
    }
    
    #endregion
}