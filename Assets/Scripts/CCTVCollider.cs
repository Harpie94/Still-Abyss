using UnityEngine;
using UnityEngine.Events;

public class CCTVCollider : MonoBehaviour
{
    public UnityEvent PlayerLeaveCCTVCollider;
    public UnityEvent PlayerEntersCCTVCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerEntersCCTVCollider?.Invoke();
            Debug.Log("Player entered CCTV Collider");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLeaveCCTVCollider?.Invoke();
            Debug.Log("Player left CCTV Collider");
        }
    }
}
