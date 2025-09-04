using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    public GameObject leftPart;
    public GameObject rightPart;
    public bool isOpen { get; private set; }
    [SerializeField] private bool front;

    void Start()
    {
        isOpen = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoorTrigger()
    {
        if (isOpen)
        {
            StartCoroutine(DoorMovement(1));
            isOpen = false;
        }
        else
        {
            StartCoroutine(DoorMovement(-1));
            isOpen = true;
        }
    }

    IEnumerator DoorMovement(int direction)
    {
        float distancePerTick = 0.05f;
        for (int i = 0; i < 20; i++)
        {
            Vector3 leftPosition = leftPart.transform.position;
            if (front)
            {
            leftPosition.x += distancePerTick*direction;
            }
            else
            {
            leftPosition.z += distancePerTick*direction;
            }
            leftPart.transform.position = leftPosition;

            Vector3 rightPosition = rightPart.transform.position;
            if (front)
            {
            rightPosition.x -= distancePerTick*direction;
            }
            else
            {
            rightPosition.z -= distancePerTick*direction;
            }
            rightPart.transform.position = rightPosition;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
