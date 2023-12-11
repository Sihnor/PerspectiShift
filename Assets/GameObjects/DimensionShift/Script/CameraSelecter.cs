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
        [Header("Camera"), SerializeField] public Camera ThreeDCamera;
        public Camera TwoDCamera;
        public ThreeDCameraControl ThreeDCameraScript;
        public TwoDCameraControl TwoDCameraScript;

        [Header("Movement"), SerializeField] public PlayerMovement2D Movement2DScript;
        public PlayerMovement3D Movement3DScript;

        public void OnDimensionSwitch()
        {
            this.ThreeDCamera.enabled = !this.ThreeDCamera.enabled;
            this.TwoDCamera.enabled = !this.TwoDCamera.enabled;

            if (this.ThreeDCamera.enabled)
            {
                this.Movement2DScript.ViewMode = EViewMode.ThreeDimension;
                this.Movement3DScript.ViewMode = EViewMode.ThreeDimension;
                this.ThreeDCameraScript.ViewMode = EViewMode.ThreeDimension;
                this.TwoDCameraScript.ViewMode = EViewMode.ThreeDimension;
            }

            if (this.TwoDCamera.enabled)
            {
                this.Movement3DScript.ViewMode = EViewMode.TwoDimension;
                this.Movement2DScript.ViewMode = EViewMode.TwoDimension;
                this.TwoDCameraScript.ViewMode = EViewMode.TwoDimension;
                this.ThreeDCameraScript.ViewMode = EViewMode.TwoDimension;
            }
        }
    }
}