using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class DimensionControl : MonoBehaviour
    {
        [SerializeField] public CameraSelecter DimensionManager;

        [SerializeField] public int Cooldown = 1;

        private float TimeForNextSwitch = 0;

        [SerializeField] public bool HasDimensionGear;

        private PlayerInput PlayerInput;
        private InputAction DimensionGearAction;

        private void Awake()
        {
            this.PlayerInput = GetComponent<PlayerInput>();
            this.DimensionGearAction = this.PlayerInput.currentActionMap.FindAction("UseDimensionGear");
        }

        private void Start()
        {
            this.DimensionGearAction.started += DimensionGearUse;
        }

        public void CollectDimensionGear()
        {
            this.HasDimensionGear = true;
        }

        public void RemoveDimensionGear()
        {
            this.HasDimensionGear = false;
        }

        public void DimensionGearUse(InputAction.CallbackContext _context)
        {
            if (!this.HasDimensionGear) return;
                        
            if (this.TimeForNextSwitch > Time.time) return;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f, LayerMask.GetMask("ShadowWall")))
            {
                Debug.DrawLine(transform.position, transform.position + transform.forward * 2f, Color.red, 1f);
                this.DimensionManager.TwoDCameraScript.SetRotationToWall(hit.normal);
                Debug.Log("Hit");
            }

            

            
            this.DimensionManager.OnDimensionSwitch();
            this.TimeForNextSwitch = Time.time + this.Cooldown;
        }
    }
}