using UnityEngine;

public class CameraMinimap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Player;
    public float distanceFromPlayer = 40f;

    private void Start()
    {
        if (Player == null)
        {
            Debug.LogError("Player GameObject is not assigned in the CameraMinimap script.");
        }
    }

    private void LateUpdate()
    {
        if (Player != null)
        {
            transform.position = new Vector3(Player.transform.position.x, distanceFromPlayer, Player.transform.position.z);
        }
    }
}
