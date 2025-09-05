using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private bool isPaused;

    private InputSystem_Actions playerControls;
    private InputAction menu;



    void Awake()
    {
        playerControls = new InputSystem_Actions();
    }


    void Update()
    {
    }

    private void OnEnable()
    {
        // Enable the whole "Menu" action map
        playerControls.Pause.Enable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Get the specific action (Escape)
        menu = playerControls.Pause.Escape;


        // Subscribe to the action
        menu.performed += Pause;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        menu.performed -= Pause;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Disable the whole "Menu" action map
        playerControls.Pause.Disable();
    }


    void Pause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    void ActivateMenu()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }

    void DeactivateMenu()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        isPaused = false;


    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void ResumeGame()
    {
        DeactivateMenu();
    }

}