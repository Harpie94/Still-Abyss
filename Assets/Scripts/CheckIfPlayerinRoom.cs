using UnityEngine;

public class CheckIfPlayerinRoom : MonoBehaviour
{

    public GameObject Player;
    public GameObject Room;
    public float distanceThreshold = 7f;
    public bool PlayerInRoom = false;
    public GameObject AffectedMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.transform.position, Room.transform.position) < distanceThreshold)
        {
            PlayerInRoom = true;
            AffectedMesh.SetActive(false);
        }
        else
        {
            PlayerInRoom = false;
            AffectedMesh.SetActive(true);
        }
    }
}
