using System;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject Doors;
    private DoorScript doorScript;
    public bool canInteract = false;

    private void Start()
    {

        if (Doors == null)
        {
            Debug.LogWarning("Button doesn't have any doors assigned");
        }
        doorScript = Doors.GetComponent<DoorScript>();

    }


    //Check if player entered the trigger
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == Player)
        {
            Debug.Log("Button can be pressed");
            canInteract = true;
        }
        else if (collider.gameObject != Player)
        {
            Debug.Log("Something got in the trigger");
        }
    }

    //Check if player exited the trigger
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == Player)
        {
            Debug.Log("Button cannot");
            canInteract = false;
        }
        else if (collider.gameObject != Player)
        {
            Debug.Log("Something left the trigger");
        }
    }

    //Calls the function in DoorScript to open/close the door
    public void Interact()
    {
        if (canInteract) 
        {
            Debug.Log("Button interacted with");
            doorScript.DoorTrigger();
        }

    }
}
