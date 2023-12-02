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

            this.DimensionManager.OnDimensionSwitch();
            this.TimeForNextSwitch = Time.time + this.Cooldown;
        }
    }
}