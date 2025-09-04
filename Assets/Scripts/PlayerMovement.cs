using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Controls;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float crouchMultiplier = 0.5f;

    [Header("Look Sensitivity")]
    [SerializeField] private float lookSensitivity = 1f;
    [SerializeField] private float lookRange = 80f;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 5f;
    [SerializeField] private float raycastCheckInterval = 0.1f; // Vérification tous les 0.1s au lieu de chaque frame

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
    private InputAction pauseAction;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private CharacterController characterController;
    private float originalHeight;
    private float crouchHeight = 1f;

    private float gravity = -9.81f;
    private float verticalVelocity = 0f;

    public float mouseYRotation;
    public bool tabletState = false;

    private RepairableObject currentRepairableObject;
    private bool isRepairing = false;
    private List<RepairableObject> nearbyRepairableObjects = new List<RepairableObject>();
    private Coroutine raycastCheckCoroutine;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing from the player object.");
        }

        originalHeight = characterController.height;

        var PlayerActionMap = PlayerControls.FindActionMap(PlayerActionMapName);
        moveAction = PlayerActionMap.FindAction("Move");
        lookAction = PlayerActionMap.FindAction("Look");
        sprintAction = PlayerActionMap.FindAction("Sprint");
        crouchAction = PlayerActionMap.FindAction("Crouch");
        interactAction = PlayerActionMap.FindAction("Interact");
        tabletToggle = PlayerActionMap.FindAction("Tablet");
        pauseAction = PlayerActionMap.FindAction("Pause");

        if (moveAction == null)
        {
            Debug.LogError("Move action found:");
        }

        if (pauseAction != null)
            pauseAction.wantsInitialStateCheck = true;

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
        interactAction.started += OnInteractStarted;
        interactAction.canceled += OnInteractCanceled;
        tabletToggle.Enable();
        tabletToggle.performed += ctx => OnTabletToggle();
        pauseAction.Enable();
        pauseAction.performed += ctx => OnPauseToggle();

        // Démarrer la vérification périodique des objets réparables
        raycastCheckCoroutine = StartCoroutine(PeriodicRaycastCheck());
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        crouchAction.Disable();
        interactAction.Disable();
        interactAction.started -= OnInteractStarted;
        interactAction.canceled -= OnInteractCanceled;
        tabletToggle.Disable();
        tabletToggle.performed -= ctx => OnTabletToggle();
        pauseAction.Disable();
        pauseAction.performed -= ctx => OnPauseToggle();

        // Arrêter la vérification périodique
        if (raycastCheckCoroutine != null)
        {
            StopCoroutine(raycastCheckCoroutine);
            raycastCheckCoroutine = null;
        }
    }

    private void Update()
    {
        if (tabletState || GameManager.Instance.CurrentState == GameManager.GameState.Paused)
            return;

        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        HandleMovement();
        HandleRotation();
    }

    private IEnumerator PeriodicRaycastCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(raycastCheckInterval);

            if (!tabletState && GameManager.Instance.CurrentState == GameManager.GameState.InGame)
            {
                CheckForRepairableObject();
            }
        }
    }

    private void CheckForRepairableObject()
    {
        RepairableObject closestRepairable = null;
        float closestDistance = float.MaxValue;

        foreach (RepairableObject repairable in nearbyRepairableObjects)
        {
            if (repairable != null && repairable.CanBeRepaired)
            {
                float distance = Vector3.Distance(transform.position, repairable.transform.position);
                if (distance < closestDistance && distance <= interactionRange)
                {
                    // Vérification d'angle simplifiée sans raycast
                    Vector3 directionToObject = (repairable.transform.position - playerCamera.transform.position).normalized;
                    float angle = Vector3.Angle(playerCamera.transform.forward, directionToObject);
                
                    if (angle <= 90f) // L'objet est dans le champ de vision
                    {
                        closestRepairable = repairable;
                        closestDistance = distance;
                    }
                }
            }
        }

        if (currentRepairableObject != closestRepairable)
        {
            currentRepairableObject = closestRepairable;

            if (currentRepairableObject != null)
            {
                Debug.Log($"Objet réparable détecté : {currentRepairableObject.name}");
            }
            else
            {
                Debug.Log("Aucun objet réparable à portée");
            }
        }
    }

    private bool IsObjectInLineOfSight(RepairableObject repairable)
    {
        Vector3 directionToObject = (repairable.transform.position - playerCamera.transform.position).normalized;
        float angle = Vector3.Angle(playerCamera.transform.forward, directionToObject);

        // Augmenter l'angle de vue pour être moins restrictif
        if (angle > 90f) return false;

        // Raycast pour vérifier s'il n'y a pas d'obstacle
        Ray ray = new Ray(playerCamera.transform.position, directionToObject);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            // Vérifier si on touche l'objet réparable ou son parent
            RepairableObject hitRepairable = hit.collider.GetComponent<RepairableObject>();
            if (hitRepairable == null)
                hitRepairable = hit.collider.GetComponentInParent<RepairableObject>();

            return hitRepairable == repairable;
        }

        return false;
    }

    private void OnInteractStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnInteractStarted called");
        Debug.Log($"currentRepairableObject: {(currentRepairableObject != null ? currentRepairableObject.name : "null")}");

        if (currentRepairableObject != null)
        {
            Debug.Log($"CanBeRepaired: {currentRepairableObject.CanBeRepaired}");
            Debug.Log($"IsInRepairRange: {currentRepairableObject.IsInRepairRange(transform.position)}");
            Debug.Log($"Distance: {Vector3.Distance(transform.position, currentRepairableObject.transform.position)}");
        }

        if (currentRepairableObject != null &&
            currentRepairableObject.CanBeRepaired &&
            currentRepairableObject.IsInRepairRange(transform.position))
        {
            isRepairing = true;
            currentRepairableObject.StartRepair();
            Debug.Log($"Début de la réparation de {currentRepairableObject.name}");
        }
        else
        {
            Debug.Log("Conditions de réparation non remplies");
        }
    }

    private void OnInteractCanceled(InputAction.CallbackContext ctx)
    {
        if (isRepairing && currentRepairableObject != null)
        {
            isRepairing = false;
            currentRepairableObject.StopRepair();
            Debug.Log($"Arrêt de la réparation de {currentRepairableObject.name}");
        }
    }

    private void HandleMovement()
    {
        bool isCrouching = crouchAction.ReadValue<float>() > 0;
        bool isSprinting = sprintAction.ReadValue<float>() > 0 && !isCrouching;

        characterController.height = isCrouching ? crouchHeight : originalHeight;
        characterController.center = new Vector3(0, characterController.height / 2f, 0);

        float speedMultiplier = GetSpeedMultiplier(isCrouching, isSprinting);

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= walkSpeed * speedMultiplier;

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

    public void SetControlsEnabledUsingTablet(bool enabled)
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

    private void OnPauseToggle()
    {
        Debug.Log("Pause Toggle");
        GameManager.Instance.TogglePause();
    }

    public bool GetTabletState(bool state)
    {
        return tabletState;
    }
    

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Player wants to interact");
        
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log($"OnTriggerEnter: {collider.name}");
        RepairableObject repairable = collider.GetComponent<RepairableObject>();
        if (repairable != null && !nearbyRepairableObjects.Contains(repairable))
        {
            nearbyRepairableObjects.Add(repairable);
        }
        switch (collider.tag)
        {
            case "deck":
                DefenseManager.SetActualRoom(0);
                break;
            case "cctv":
                DefenseManager.SetActualRoom(1);
                break;
            case "storage":
                DefenseManager.SetActualRoom(2);
                break;
            case "restroom":
                DefenseManager.SetActualRoom(3);
                break;
            case "lounge":
                DefenseManager.SetActualRoom(4);
                break;
            case "lab":
                DefenseManager.SetActualRoom(5);
                break;
            case "kitchen":
                DefenseManager.SetActualRoom(6);
                break;
            case "bedroom":
                DefenseManager.SetActualRoom(7);
                break;
            case "sonar":
                DefenseManager.SetActualRoom(8);
                break;
            case "gear":
                DefenseManager.SetActualRoom(9);
                break;
            case "o2":
                DefenseManager.SetActualRoom(10);
                break;
            case "moonpool":
                DefenseManager.SetActualRoom(11);
                break;
            default:
                Debug.Log("failed");
                break;
        }
        //if (collider.CompareTag("deck"))
    }

    void OnTriggerExit(Collider collider)
    {
        RepairableObject repairable = collider.GetComponent<RepairableObject>();
        if (repairable != null && nearbyRepairableObjects.Contains(repairable))
        {
            nearbyRepairableObjects.Remove(repairable);
            
            // Si c'était l'objet actuellement sélectionné, le désélectionner
            if (currentRepairableObject == repairable)
            {
                currentRepairableObject = null;
                // TODO: Cacher UI d'interaction
            }
        }
        DefenseManager.SetActualRoom(12);
    }

}
