using System.Collections;
using System.Collections.Generic;
using Player.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSelecter : MonoBehaviour
{
    [Header("Camera"), SerializeField] public Camera FirstPerson;
    public Camera ThirdPerson;

    [Header("Movement"), SerializeField] public PlayerMovement2D Movement2DScript;
    public PlayerMovement3D Movement3DScript;

    public void OnDimensionSwitch()
    {
        FirstPerson.enabled = !FirstPerson.enabled;
        ThirdPerson.enabled = !ThirdPerson.enabled;


        if (FirstPerson.enabled)
        {
            Movement2DScript.SetViewMode(Movement.EViewMode.ThreeDimension);
            Movement3DScript.SetViewMode(Movement.EViewMode.ThreeDimension);
        }

        if (ThirdPerson.enabled)
        {
            Movement2DScript.SetViewMode(Movement.EViewMode.TwoDimension);
            Movement3DScript.SetViewMode(Movement.EViewMode.TwoDimension);
        }
    }
}