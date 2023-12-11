using System;
using System.Collections;
using System.Collections.Generic;
using Player.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragShadowObject : MonoBehaviour
{
    private PlayerInput PlayerInput;
    private InputAction DragAction;
    private bool IsDragging = false;

    private RaycastHit Hit;
    private Vector3 DragOffset = Vector3.zero;

    private PlayerMovement3D PlayerMovement3D;
    private ThreeDCameraControl ThreeDCameraControl;
    
    private void Awake()
    {
        this.PlayerInput = GetComponent<PlayerInput>();
        this.DragAction = this.PlayerInput.currentActionMap.FindAction("DragShadowObject");
        
        this.PlayerMovement3D = GetComponent<PlayerMovement3D>();
        this.ThreeDCameraControl = GetComponentInChildren<ThreeDCameraControl>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.DragAction.started += DragObject;
        this.DragAction.canceled += StopDragObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsDragging)
        {
            this.Hit.collider.transform.position = transform.position + this.DragOffset;
        }
    }

    private void DragObject(InputAction.CallbackContext _context)
    {
        if (Physics.Raycast(transform.position, transform.forward, out this.Hit, 10f, LayerMask.GetMask("ShadowObject")) 
            && this.Hit.collider.gameObject.CompareTag($"Draggable"))
        {
            if (this.Hit.collider.gameObject.GetComponent<Draggable>().GetIsDraggable() == false) return;
            
            this.DragOffset = this.Hit.collider.transform.position - transform.position;
            this.DragOffset -= this.Hit.normal * 0.02f;
            this.IsDragging = true;

            this.PlayerMovement3D.SetIsDragging(true);
            this.ThreeDCameraControl.SetIsDragging(true);
        }
        else
        {
            this.IsDragging = false;
            this.PlayerMovement3D.SetIsDragging(false);
            this.ThreeDCameraControl.SetIsDragging(false);
        }
    }
    
    private void StopDragObject(InputAction.CallbackContext _context)
    {
        this.IsDragging = false;
        this.PlayerMovement3D.SetIsDragging(false);
        this.ThreeDCameraControl.SetIsDragging(false);
    }
    
    public bool IsDragged()
    {
        return this.IsDragging;
    }
    
    public RaycastHit GetGrabbedObject()
    {
        return this.Hit;
    }
}