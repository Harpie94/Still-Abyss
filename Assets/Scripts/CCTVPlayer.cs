using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CCTVPlayer : MonoBehaviour
{

    [Header("Input Action")]
    [SerializeField] private InputActionAsset PlayerControls;
    [SerializeField] private string CCTVActionName = "Camera";
    private InputActionMap CCTVActionMap;


    [Header("Linked Components")]
    [SerializeField] private Canvas TabletCanva;
    public GameObject playerCamera;
    public GameObject cctvCamera;
    public GameObject playerLight;
    public GameObject CCTVTrigger;

    [Header("Events")]
    public UnityEvent PlayerLeaveCCTV;
    public UnityEvent PlayerEntersCCTV;

    private InputAction leaveCCTV;
    public bool inCollider = false;
    public bool isLookingAtCams = false;


    private void Awake()
    {
        cctvCamera.SetActive(false);
        CCTVActionMap = PlayerControls.FindActionMap(CCTVActionName);
        leaveCCTV = CCTVActionMap.FindAction("LeaveCCTV");
        leaveCCTV.performed += ctx => LeaveCCTV();
        leaveCCTV.Disable();

        if (TabletCanva = null)
        { 
            Debug.LogWarning("TabletCanva is not assigned or is null in CCTV Script.");
        }
        if (playerCamera == null)
        {
            Debug.LogWarning("playerCamera is not assigned or is null in CCTV Script.");
        }
        if (cctvCamera == null)
        {
            Debug.LogError("playerCamera is not assigned or is null in CCTV Script.");
        }
        if (CCTVTrigger == null)
        {
           Debug.LogError("CCTVTrigger is not assigned or is null in CCTV Script.");
        }

    }

    public void PlayerInCollider()
    {
        Debug.Log("Player in CCTV Collider");
        inCollider = true;
    }

    public void PlayerOutOfCollider()
    {
        Debug.Log("Player out of CCTV Collider");
        inCollider = false;

    }

    public void EnterCCTV()
    {
        if (inCollider) 
        {
            PlayerEntersCCTV?.Invoke();
            leaveCCTV.Enable();
            playerLight.SetActive(false);
            playerCamera.SetActive(false);
            cctvCamera.SetActive(true);
            isLookingAtCams = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            TabletCanva.enabled = false;
        }
        

    }

    public void LeaveCCTV()
    {
        PlayerLeaveCCTV?.Invoke();
        playerLight.SetActive(true);
        playerCamera.SetActive(true);
        cctvCamera.SetActive(false);
        isLookingAtCams = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        leaveCCTV.Disable();

    }

}
