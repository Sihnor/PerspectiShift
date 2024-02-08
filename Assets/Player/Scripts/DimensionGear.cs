using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class DimensionGear : MonoBehaviour
    {
        [SerializeField] private CameraControl2D CameraControl2D;

        [SerializeField] public int Cooldown = 1;

        private float TimeForNextSwitch = 0;
        
        private InputAction DimensionGearAction;

        private bool LastFrameDimensionGearAvailable = false;

        private void Awake()
        {
            this.DimensionGearAction = GetComponent<PlayerInput>().currentActionMap.FindAction("UseDimensionGear");
        }

        private void Start()
        {
            this.DimensionGearAction.started += DimensionGearUse;
        }

        private void Update()
        {

            UpdateDimensionGearUI();
        }

        public void CollectDimensionGear()
        {
            EventManager.Instance.OnDimensionGearPosses(true);
        }

        public void RemoveDimensionGear()
        {
            EventManager.Instance.OnDimensionGearPosses(false);
        }

        public void DimensionGearUse(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.HasDimensionGear) return;

            if (this.TimeForNextSwitch > Time.time) return;

            EventManager.Instance.OnDimensionSwitch();
            
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f, LayerMask.GetMask("ShadowWall")))
                this.CameraControl2D.SetRotationToWall(hit.normal);
            
            
            this.TimeForNextSwitch = Time.time + this.Cooldown;
        }

        private void UpdateDimensionGearUI()
        {
            bool isDimensionGearAvailable = Physics.Raycast(transform.position, transform.forward, 2f, LayerMask.GetMask("ShadowWall"));
            if (isDimensionGearAvailable == this.LastFrameDimensionGearAvailable) return;
            
            EventManager.Instance.OnDimensionGearAvailable(isDimensionGearAvailable);
            this.LastFrameDimensionGearAvailable = isDimensionGearAvailable;
        }
    }
}