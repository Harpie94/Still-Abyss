using UnityEngine;

public class DefenseManager : MonoBehaviour
{
    private bool[] roomShutterState = new bool[12]; //true = volet fermé, false = volet ouvert

    /*liste des salles :
    0 : observation deck
    1 : cctv
    2 : storage
    3 : restroom
    4 : lounge
    5 : experimentation room/laboratory
    6 : kitchen/living room
    7 : bedroom
    8 : sonar
    9 : diving gear
    10 : o2
    11 : moonpool */

    public GameObject[] iconRooms = new GameObject[12];

    void Start()
    {
        for (int i = 0; i < 12; i++)
        {
            roomShutterState[i] = false;
            iconRooms[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            ShutterTrigger(4);
        }
    }

    bool GetShutterState(int room)
    {
        if (room < roomShutterState.Length)
        {
            return roomShutterState[room];
        }
        else
        {
            Debug.Log("Int too high, no room found at this index");
            return false;
        }
    }

    public void ShutterTrigger(int room)
    {
        Debug.Log("activate");
        if (room < roomShutterState.Length)
        {
            roomShutterState[room] = !roomShutterState[room];
            RoomIconTrigger(room);
        }
        else
        {
            Debug.Log("Int too high, no room found at this index");
        }
    }

    void RoomIconTrigger(int room)
    {
        if (roomShutterState[room])
        {
            iconRooms[room].SetActive(true);
        }
        else
        {
            iconRooms[room].SetActive(false);
        }
    }
}
