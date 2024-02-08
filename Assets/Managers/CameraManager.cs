using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Player.Scripts;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera"), SerializeField] 
    public Camera TwoDCamera;
    public Camera ThreeDCamera;
    public CameraControl2D CameraScript2D;
    public CameraControl3D CameraScript3D;

    [Header("Movement"), SerializeField] public PlayerMovement2D Movement2DScript;
    public PlayerMovement3D Movement3DScript;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.FOnDimensionSwitch += OnDimensionSwitch;
    }

    private void OnDimensionSwitch(bool isSwitch)
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