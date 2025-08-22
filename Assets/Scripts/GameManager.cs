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
    }

    void OnDisable()
    {
        OnGameStateChanged -= HandleGameStateChange;
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
}