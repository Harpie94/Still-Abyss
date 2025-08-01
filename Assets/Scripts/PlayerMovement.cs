using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float crouchMultiplier = 0.5f;

    [Header("Look Sensitivity")]
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float lookRange = 80f;


    [Header("Input Action")]
    [SerializeField] private InputActionAsset PlayerControls;
    [SerializeField] private string PlayerActionMapName = "Player";

    [Header("Linked Components")]
    [SerializeField] private Canvas TabletCanva;


    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction crouchAction;
    private InputAction sprintAction;
    private InputAction interactAction;
    private InputAction tabletToggle;
    private Vector2 moveInput;
    private Vector2 lookInput;


    public CinemachineCamera playerCamera;
    public float mouseYRotation;
    private CharacterController characterController;
    private float originalHeight;
    private float crouchHeight = 1f;
    private float gravity = -9.81f;
    private float verticalVelocity = 0f;
    public bool tabletState = false;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing from the player object.");
        }

        originalHeight = characterController.height; // Stocke la hauteur d'origine

        var PlayerActionMap = PlayerControls.FindActionMap(PlayerActionMapName);
        moveAction = PlayerActionMap.FindAction("Move");
        lookAction = PlayerActionMap.FindAction("Look");
        sprintAction = PlayerActionMap.FindAction("Sprint");
        crouchAction = PlayerActionMap.FindAction("Crouch");
        interactAction = PlayerActionMap.FindAction("Interact");
        tabletToggle = PlayerActionMap.FindAction("Tablet");

        if (moveAction == null)
        {
            Debug.LogError("Move action found:");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (TabletCanva != null)
        {
            TabletCanva.enabled = false;
        }
        else
        {
            Debug.LogWarning("TabletCanva is not assigned or is null.");
        }
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();
        interactAction.Enable();
        interactAction.started += OnInteractPerformed;
        tabletToggle.Enable();
        tabletToggle.performed += ctx => OnTabletToggle();
    }
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        crouchAction.Disable();
        interactAction.Disable();
        interactAction.started -= OnInteractPerformed;
        tabletToggle.Disable();
        tabletToggle.performed -= ctx => OnTabletToggle();
    }

    private void FixedUpdate()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();
        Move();
        Rotation();
    }



    public void OnTabletToggle()
    {
        Debug.Log("Tablet toggle");
        tabletState = !tabletState;
        switch (tabletState)
        {
            case true:
                ShowTablet();
                break;
            case false:
                HideTablet();
                break;
        }
    }

    public void ShowTablet()
    {
        Debug.Log("Show tablet");
        moveAction.Disable();
        lookAction.Disable();
        crouchAction.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (TabletCanva != null)
        {
            TabletCanva.enabled = true;
        }
        else
        {
            Debug.LogWarning("TabletCanva is not assigned or is null.");
        }
    }

    public void HideTablet()
    {
        Debug.Log("Hide tablet");
        moveAction.Enable();
        lookAction.Enable();
        crouchAction.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (TabletCanva != null)
        {
            TabletCanva.enabled = false;         
        }
        else
        {
            Debug.LogWarning("TabletCanva is not assigned or is null.");
        }
    }

    public bool GetTabletState(bool state)
    {
        return tabletState;
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Player wants to interact");
        
    }


    private void Move()
    {
        bool isCrouching = crouchAction.ReadValue<float>() > 0;
        bool isSprinting = sprintAction.ReadValue<float>() > 0 && !isCrouching;

        // Ajuste la hauteur du CharacterController
        characterController.height = isCrouching ? crouchHeight : originalHeight;

        float speedMultiplier = 1f;
        if (isCrouching)
        {
            speedMultiplier = crouchMultiplier;
        }
        else if (isSprinting)
        {
            speedMultiplier = sprintMultiplier;
        }

        float verticalSpeed = moveInput.y * walkSpeed * speedMultiplier;
        float horizontalSpeed = moveInput.x * walkSpeed * speedMultiplier;

        Vector3 move = new Vector3(horizontalSpeed, 0, verticalSpeed);
        move = transform.rotation * move;

        // Gestion de la gravit�
        if (characterController.isGrounded)
        {
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);
    }

    private void Rotation()
    {
        float mouseXRotation = lookInput.x * lookSensitivity;
        transform.Rotate(0, mouseXRotation, 0);
        mouseYRotation -= lookInput.y * lookSensitivity;
        mouseYRotation = Mathf.Clamp(mouseYRotation, -lookRange, lookRange);
        playerCamera.transform.localRotation = Quaternion.Euler(mouseYRotation, 0, 0);
    }

}
