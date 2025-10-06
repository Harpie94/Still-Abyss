using UnityEngine;
using UnityEngine.Events;

public class CameraAwake : MonoBehaviour
{

    
    [Header("CameraID")]
    public int cameraId = 0;

    [Header("Events")]
    public UnityEvent<int> CameraTurnedOn;
    public UnityEvent<int> CameraTurnedOff;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        CameraTurnedOn?.Invoke(cameraId);
        Debug.Log($"Camera with id {cameraId} turned on.");
    }

    private void OnDisable()
    {
        CameraTurnedOff?.Invoke(cameraId);
        Debug.Log($"Camera with id {cameraId} turned off.");
    }
}
