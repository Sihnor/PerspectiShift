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
        public CameraControl3D CameraScript3D;
        public CameraControl2D CameraScript2D;

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
                this.CameraScript3D.ViewMode = EViewMode.ThreeDimension;
                this.CameraScript2D.ViewMode = EViewMode.ThreeDimension;
            }

            if (this.TwoDCamera.enabled)
            {
                this.Movement3DScript.ViewMode = EViewMode.TwoDimension;
                this.Movement2DScript.ViewMode = EViewMode.TwoDimension;
                this.CameraScript2D.ViewMode = EViewMode.TwoDimension;
                this.CameraScript3D.ViewMode = EViewMode.TwoDimension;
            }
        }
    }
}