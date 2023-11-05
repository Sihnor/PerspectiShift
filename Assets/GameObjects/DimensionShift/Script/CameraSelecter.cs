using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


namespace Player.Scripts
{
    public class CameraSelecter : MonoBehaviour
    {
        [Header("Camera"), SerializeField] public Camera FirstPersonCamera;
        public Camera ThirdPersonCamera;
        public FPCameraControl FirstPersonCameraScript;
        public TPCameraControl ThirdPersonCameraScript;

        [Header("Movement"), SerializeField] public PlayerMovement2D Movement2DScript;
        public PlayerMovement3D Movement3DScript;

        public void OnDimensionSwitch()
        {
            this.FirstPersonCamera.enabled = !FirstPersonCamera.enabled;
            this.ThirdPersonCamera.enabled = !ThirdPersonCamera.enabled;

            if (FirstPersonCamera.enabled)
            {
                this.Movement2DScript.ViewMode = EViewMode.ThreeDimension;
                this.Movement3DScript.ViewMode = EViewMode.ThreeDimension;
                this.FirstPersonCameraScript.ViewMode = EViewMode.ThreeDimension;
                this.ThirdPersonCameraScript.ViewMode = EViewMode.ThreeDimension;
            }

            if (ThirdPersonCamera.enabled)
            {
                this.Movement3DScript.ViewMode = EViewMode.TwoDimension;
                this.Movement2DScript.ViewMode = EViewMode.TwoDimension;
                this.ThirdPersonCameraScript.ViewMode = EViewMode.TwoDimension;
                this.FirstPersonCameraScript.ViewMode = EViewMode.TwoDimension;
            }
        }
    }
}