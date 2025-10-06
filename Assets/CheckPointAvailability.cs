using UnityEngine;

public class CheckPointAvailability : MonoBehaviour
{
    public Collider SpawnpointCollider;
    public bool isAvailable = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (SpawnpointCollider == null)
        {
            SpawnpointCollider = GetComponent<Collider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isAvailable = false;
    }
    private void OnTriggerExit(Collider other)
    {
        isAvailable = true;
    }

}


