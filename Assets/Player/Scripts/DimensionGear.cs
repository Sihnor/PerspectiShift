using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class DimensionGear : MonoBehaviour
    {
        [SerializeField] private CameraControl2D CameraControl2D;

        [SerializeField] public int Cooldown = 1;

        private float TimeForNextSwitch;
        
        private InputAction DimensionGearAction;

        private bool LastTimeDimensionGearAvailable;
        
        private bool Is2D = false;
        
        private void Awake()
        {
            this.DimensionGearAction = GetComponent<PlayerInput>().currentActionMap.FindAction("UseDimensionGear");
            
            EventManager.Instance.FOnDimensionSwitch += OnDimensionSwitch;
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
            
            bool didItHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f, LayerMask.GetMask("ShadowWall"));
            Debug.DrawLine(transform.position, transform.position + transform.forward * 2, Color.red, 2f);
            if (!didItHit) return;
            
            EventManager.Instance.OnDimensionSwitch();
            
            // When 2D the player is placed on the wall and when 3D the player is placed on the floor
            if (this.Is2D) transform.position = hit.point + hit.normal * 0.3f;
            else transform.position = hit.point + hit.normal * 2f;
            
            this.CameraControl2D.SetRotationToWall(hit.normal);
            
            this.TimeForNextSwitch = Time.time + this.Cooldown;
        }

        private void UpdateDimensionGearUI()
        {
            bool isDimensionGearAvailable = Physics.Raycast(transform.position, transform.forward, 2f, LayerMask.GetMask("ShadowWall"));
            if (isDimensionGearAvailable == this.LastTimeDimensionGearAvailable) return;
            
            EventManager.Instance.OnDimensionGearAvailable(isDimensionGearAvailable);
            this.LastTimeDimensionGearAvailable = isDimensionGearAvailable;
        }
        
        private void OnDimensionSwitch()
        {
            this.Is2D = !this.Is2D;
        }
    }
}