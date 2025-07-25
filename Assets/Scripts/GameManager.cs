using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager Instance;

    public enum GameState
    {
        InGame,
        Paused,
        Dead
    }

    public enum DayNightState
    {
        Day,
        Night
    }

    public GameObject player;
    public string playerTag = "Player";

    // Cycle jour/nuit
    public DayNightState CurrentDayNightState { get; private set; } = DayNightState.Day;
    public float phaseDuration = 420f; // 7 minutes en secondes
    private float phaseTimer = 0f;
    public int maxCycles = 5;
    private int currentCycle = 1;
    public bool endlessMode = false;
    public Light sunLight; // Assigner la Directional Light dans l'inspecteur

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

    void Update()
    {
        // Gestion du cycle jour/nuit uniquement en jeu
        if (CurrentState != GameState.InGame) return;

        phaseTimer += Time.deltaTime;
        if (phaseTimer >= phaseDuration)
        {
            phaseTimer = 0f;
            SwitchDayNight();
        }

        // Transition d’intensité de la lumière (exemple simple)
        if (sunLight != null)
        {
            float t = phaseTimer / phaseDuration;
            if (CurrentDayNightState == DayNightState.Day)
                sunLight.intensity = Mathf.Lerp(1f, 0.2f, t); // Jour → Nuit
            else
                sunLight.intensity = Mathf.Lerp(0.2f, 1f, t); // Nuit → Jour
        }
    }
    
    #endregion

    #region Cycle Management

    void SwitchDayNight()
    {
        if (CurrentDayNightState == DayNightState.Day)
        {
            CurrentDayNightState = DayNightState.Night;
        }
        else
        {
            CurrentDayNightState = DayNightState.Day;
            currentCycle++;
            if (!endlessMode && currentCycle > maxCycles)
            {
                SetGameState(GameState.Dead);
            }
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
            var movement = player.GetComponent<PlayerMovementDay>();
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

    private void ApplyPause(CharacterController controller, PlayerMovementDay movement)
    {
        if (controller != null) controller.enabled = false;
        if (movement != null) movement.enabled = false;
        Time.timeScale = 0f;
        // TODO: AJOUTER LE MENU PAUSE
    }

    private void ApplyDead(CharacterController controller, PlayerMovementDay movement)
    {
        if (controller != null) controller.enabled = false;
        if (movement != null) movement.enabled = false;
        Time.timeScale = 0f;
        // TODO: AJOUTER L'ECRAN DE MORT
    }

    private void ApplyResume(CharacterController controller, PlayerMovementDay movement)
    {
        if (controller != null) controller.enabled = true;
        if (movement != null) movement.enabled = true;
        Time.timeScale = 1f;
    }

    void OnEnable()
    {
        OnGameStateChanged += HandleGameStateChange;
    }

    void OnDisable()
    {
        OnGameStateChanged -= HandleGameStateChange;
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