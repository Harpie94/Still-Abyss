using UnityEngine;

public class ShutterScript : MonoBehaviour
{
    [Header("Variables")]
    public Animator shutterAnimator;
    public bool isOpen = true;
    public bool isActive = false;
    public int shutterId = 0;

    [Header("Camera Reference")]
    public CameraAwake linkedCamera;

    private void OnEnable()
    {
        if (linkedCamera != null)
        {
            linkedCamera.CameraTurnedOn.AddListener(OnCameraTurnedOn);
            linkedCamera.CameraTurnedOff.AddListener(OnCameraTurnedOff);
        }
        else
        {
            Debug.LogWarning("CameraAwake reference not set on ShutterScript.");
        }
    }

    void Start()
    {
        if (shutterAnimator == null)
        {
            Debug.LogError("No animator assigned to the shutter, please assign one in the inspector.");
        }
    }

    public void OnCameraTurnedOn(int cameraId)
    {
        Debug.Log($"Caméra {cameraId} activée.");
        if (cameraId == shutterId)
        {
            isActive = true;
        }
    }

    public void OnCameraTurnedOff(int cameraId)
    {
        Debug.Log($"Caméra {cameraId} désactivée.");
        if (cameraId == shutterId)
        {
            isActive = false;
        }
    }

    public void ShutterTrigger()
    {
        if (isActive)
        {
            isOpen = !isOpen;
            if (shutterAnimator != null)
            {
                shutterAnimator.SetBool("IsOpen", isOpen);
            }
        }
    }
}
