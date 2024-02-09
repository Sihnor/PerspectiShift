using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

enum EDoorState
{
    Closed,
    Opening,
    Closing
}

public class SlidingDoorScript : MonoBehaviour
{
    [SerializeField] private GameObject LeftDoor;
    private Vector3 LeftDoorStartTransform;
    [SerializeField] private GameObject RightDoor;
    private Vector3 RightDoorStartTransform;

    private EDoorState DoorState = EDoorState.Closed;


    private void OpenDoor()
    {
        this.DoorState = EDoorState.Opening;
    }

    private void CloseDoor()
    {
        this.DoorState = EDoorState.Closing;
    }

    public void ToggleDoor()
    {
        if (this.DoorState == EDoorState.Opening)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleDoor();
        }
    }

    private void Awake()
    {
        this.LeftDoorStartTransform = this.LeftDoor.transform.position;
        this.RightDoorStartTransform = this.RightDoor.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.DoorState == EDoorState.Opening)
        {
            this.LeftDoor.transform.position = Vector3.Lerp(this.LeftDoor.transform.position, this.LeftDoorStartTransform + this.LeftDoor.transform.up * -1, Time.deltaTime);
            this.RightDoor.transform.position = Vector3.Lerp(this.RightDoor.transform.position, this.RightDoorStartTransform + this.RightDoor.transform.up, Time.deltaTime);
        }
        
        if (this.DoorState == EDoorState.Closing)
        {
            this.LeftDoor.transform.position = Vector3.Lerp(this.LeftDoor.transform.position, this.LeftDoorStartTransform, Time.deltaTime);
            this.RightDoor.transform.position = Vector3.Lerp(this.RightDoor.transform.position, this.RightDoorStartTransform, Time.deltaTime);
        }
    }
}
