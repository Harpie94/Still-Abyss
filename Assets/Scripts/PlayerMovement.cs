using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Controls;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float crouchMultiplier = 0.5f;

    [Header("Look Sensitivity")]
    [SerializeField] private float lookSensitivity = 1f;
    [SerializeField] private float lookRange = 80f;


    [Header("Input Action")]
    [SerializeField] private InputActionAsset PlayerControls;
    [SerializeField] private string PlayerActionMapName = "Player";

    [Header("Linked Components")]
    [SerializeField] private Canvas TabletCanva;
    public CinemachineCamera playerCamera;


    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction crouchAction;
    private InputAction sprintAction;
    private InputAction interactAction;
    private InputAction tabletToggle;
    private Vector2 moveInput;
    private Vector2 lookInput;
    
    private CharacterController characterController;
    private float originalHeight;
    private float crouchHeight = 1f;
    
    private float gravity = -9.81f;
    private float verticalVelocity = 0f;
    
    public float mouseYRotation;

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

        // Lock and hide the cursor at the start
        GameManager.Instance.SetCursorLockedState(true);
        
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

    private void Update()
    {
        if (tabletState) return;

        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        HandleMovement();
        HandleRotation();
    }
    
    private void HandleMovement()
    {
        bool isCrouching = crouchAction.ReadValue<float>() > 0;
        bool isSprinting = sprintAction.ReadValue<float>() > 0 && !isCrouching;
        
        characterController.height = isCrouching ? crouchHeight : originalHeight; // hauteur du corps
        characterController.center = new Vector3(0, characterController.height / 2f, 0);

        float speedMultiplier = GetSpeedMultiplier(isCrouching, isSprinting);

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= walkSpeed * speedMultiplier;

        // gravité
        if (characterController.isGrounded)
            verticalVelocity = 0f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);
    }
    
    private float GetSpeedMultiplier(bool isCrouching, bool isSprinting)
    {
        if (isCrouching) return crouchMultiplier;
        if (isSprinting) return sprintMultiplier;
        return 1f;
    }
    
    private void HandleRotation()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        transform.Rotate(0, mouseX, 0);

        mouseYRotation -= mouseY;
        mouseYRotation = Mathf.Clamp(mouseYRotation, -lookRange, lookRange);

        playerCamera.transform.localRotation = Quaternion.Euler(mouseYRotation, 0, 0);
    }
    
    public void OnTabletToggle()
    {
        Debug.Log("Tablet toggle");
        
        GameManager.Instance.ToggleTablet();
    }

    public void SetControlsEnabled(bool enabled)
    {
        if (enabled)
        {
            moveAction.Enable();
            lookAction.Enable();
            sprintAction.Enable();
            crouchAction.Enable();
            interactAction.Enable();
        }
        else
        {
            moveAction.Disable();
            lookAction.Disable();
            sprintAction.Disable();
            crouchAction.Disable();
            interactAction.Disable();
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

}
