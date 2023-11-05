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


        public void CollectDimensionGear()
        {
            this.HasDimensionGear = true;
        }

        public void RemoveDimensionGear()
        {
            this.HasDimensionGear = false;
        }

        public void OnDimensionGearUse(InputAction.CallbackContext _context)
        {
            if (_context.performed)
            {
                if (TimeForNextSwitch > Time.time) return;

                DimensionManager.OnDimensionSwitch();
                TimeForNextSwitch = Time.time + Cooldown;
            }
        }
    }
}