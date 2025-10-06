using UnityEngine;
using UnityEngine.UI;

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
    11 : moonpool 
    12 : not in a room */

    public GameObject[] iconRooms = new GameObject[12];
    public GameObject[] doors = new GameObject[29];
    public int actualRoom {get; private set;}

    void Start()
    {
        for (int i = 0; i < 12; i++)
        {
            roomShutterState[i] = false;
            iconRooms[i].GetComponent<Image>().color = Color.grey;
        }
    }

    // Update is called once per frame
    void Update()
    {

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
        if (room < roomShutterState.Length)
        {
            roomShutterState[room] = !roomShutterState[room];
        }
        else
        {
            Debug.Log("Int too high, no room found at this index");
        }
    }

    public void MinimapModifier(int room, int modifier)
    {
        switch (modifier)
        {
            case 0:
                iconRooms[room].GetComponent<Image>().color = Color.grey;
                break;
            case 1:
                iconRooms[room].GetComponent<Image>().color = Color.green;
                break;
            case 2:
                iconRooms[room].GetComponent<Image>().color = Color.yellow;
                break;
            case 3:
                iconRooms[room].GetComponent<Image>().color = Color.red;
                break;
            case 4:
                iconRooms[room].GetComponent<Image>().color = Color.black;
                break;
            default:
                Debug.Log("Unregistered modifier");
                break;
        }
    }

    public void SetActualRoom(int room)
    {
        actualRoom = room;
    }

    int[] GetRoomsDoors(int room)
    {
        switch (room)
        {
            case 0:
                return new int[] {0};
            case 1:
                return new int[] {1, 2};
            case 2:
                return new int[] {3, 4};
            case 3:
                return new int[] {5};
            case 4:
                return new int[] {6, 7, 8, 9};
            case 5:
                return new int[] {10, 11, 12};
            case 6:
                return new int[] {13, 14, 27, 28};
            case 7:
                return new int[] {15, 16};
            case 8:
                return new int[] {17, 18, 19};
            case 9:
                return new int[] {20, 21, 22};
            case 10:
                return new int[] {23, 26};
            case 11:
                return new int[] {24, 25};
            default:
                Debug.Log("Valeur inconnue");
                return new int[] {};
        }
    }

    void ActivateDoors(int room)
    {
        if (room < roomShutterState.Length)
        {
            int[] roomsDoors = GetRoomsDoors(room);
            for (int i = 0; i < roomsDoors.Length; i++)
            {
                int currentDoor = roomsDoors[i];
                doors[currentDoor].GetComponent<Door>().DoorTrigger();
            }
        }
        else
        {
            Debug.Log("Not a room");
        }
    }

    public void ActivateSingleDoor(int doorint)
    {
        doors[doorint].GetComponent<Door>().DoorTrigger();
    }
}
