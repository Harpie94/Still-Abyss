using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isOpen = true;
    public Animator DoorAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DoorAnimator == null)
        {
            Debug.LogError("No animator assigned to the door, please assign one in the inspector.");
        }
    }

    public void NewDoorState()
    {
           Debug.Log("Door state changed");
    }

    // Open / Close door
    public void DoorTrigger()
    {
        isOpen = !isOpen;
        DoorAnimator.SetBool("isDoorOpen", isOpen);
    }

}
