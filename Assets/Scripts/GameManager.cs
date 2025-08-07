using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region Variables
    
    [Header("UI Management")]
    [SerializeField] private Canvas tabletCanvas;

    public static GameManager Instance;

    public enum GameState
    {
        InGame,
        Paused,
        Dead
    }

    public GameObject player;
    public string playerTag = "Player";

    #endregion

    #region Basic Functions

    public GameState CurrentState { get; private set; } = GameState.InGame;

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
    
    #endregion
    
    #region UI Management
    
    public bool IsTabletOpen { get; private set; } = false;

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
                movement.SetControlsEnabled(false);
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
                movement.SetControlsEnabled(true);
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

    private void ApplyPause(CharacterController controller, PlayerMovement movement)
    {
        if (controller != null) controller.enabled = false;
        if (movement != null) movement.enabled = false;
        Time.timeScale = 0f;
        SetCursorLockedState(false); // Unlock cursor when paused
        // TODO: AJOUTER LE MENU PAUSE
    }

    private void ApplyDead(CharacterController controller, PlayerMovement movement)
    {
        if (controller != null) controller.enabled = false;
        if (movement != null) movement.enabled = false;
        Time.timeScale = 0f;
        SetCursorLockedState(false); // Unlock cursor when dead
        // TODO: AJOUTER L'ECRAN DE MORT
    }

    private void ApplyResume(CharacterController controller, PlayerMovement movement)
    {
        if (controller != null) controller.enabled = true;
        if (movement != null) movement.enabled = true;
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