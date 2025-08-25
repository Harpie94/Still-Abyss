using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    public GameObject leftPart;
    public GameObject rightPart;
    public bool isOpen { get; private set; }

    void Start()
    {
        isOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            OpenDoor();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            StartCoroutine(DoorMovement(-1));
            isOpen = true;
        }
        else
        {
            Debug.Log("Door already open");
        }
    }

    void CloseDoor()
    {
        if (isOpen)
        {
            StartCoroutine(DoorMovement(1));
            isOpen = false;
        }
        else
        {
            Debug.Log("Door already closed");
        }
    }

    IEnumerator DoorMovement(int direction)
    {
        float distancePerTick = 0.05f;
        for (int i = 0; i < 20; i++)
        {
            Vector3 leftPosition = leftPart.transform.position;
            leftPosition.x += distancePerTick*direction;
            leftPart.transform.position = leftPosition;

            Vector3 rightPosition = rightPart.transform.position;
            rightPosition.x -= distancePerTick*direction;
            rightPart.transform.position = rightPosition;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
