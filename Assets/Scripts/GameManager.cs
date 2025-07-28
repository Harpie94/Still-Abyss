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