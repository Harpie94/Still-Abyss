using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerMovementDay : MonoBehaviour
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

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing from the player object.");
        }

        originalHeight = characterController.height; // Stocke la hauteur d'origine

        var PlayerDay = PlayerControls.FindActionMap("PlayerDay");
        moveAction = PlayerDay.FindAction("Move");
        lookAction = PlayerDay.FindAction("Look");
        sprintAction = PlayerDay.FindAction("Sprint");
        crouchAction = PlayerDay.FindAction("Crouch");
        //interactAction = PlayerDay.FindAction("Interact");
        //tabletToggle = PlayerDay.FindAction("Tablet");

        if (moveAction == null)
        {
            Debug.LogError("Move action found:");
        }
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();
        //interactAction.Enable();
        //tabletToggle.Enable();
    }
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        crouchAction.Disable();
        //interactAction.Disable();
        //tabletToggle.Disable();
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();
        Move();
        Rotation();
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

        // Gestion de la gravité
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
