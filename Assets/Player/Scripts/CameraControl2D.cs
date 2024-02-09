using UnityEngine;
using UnityEngine.InputSystem;


namespace Player.Scripts
{
    public class CameraControl2D : MonoBehaviour, IViewMode
    {
        [SerializeField] public Transform Player;

        [SerializeField] private PlayerInput PlayerInput;

        private InputAction SwitchPlaneAction;
        
        private bool IsOnWall = false;
        public EViewMode ViewMode { get; set; }

        private void Awake()
        {
            this.SwitchPlaneAction = this.PlayerInput.currentActionMap.FindAction("SwitchPlane");
        }

        // Start is called before the first frame update
        private void Start()
        {
            this.SwitchPlaneAction.started += SwitchPlane;

            this.ViewMode = EViewMode.ThreeDimension;
        }

        public void SwitchPlane(InputAction.CallbackContext context)
        {
            if (this.ViewMode == EViewMode.ThreeDimension) return;
            if (this.IsOnWall) return;

            this.Player.transform.Rotate(Vector3.up, 90 * context.ReadValue<Vector2>().x);
        }

        public void SetRotationToWall(Vector3 normal)
        {
            Quaternion targetRotation = Quaternion.LookRotation( (normal * -1), this.Player.transform.up);

            this.Player.transform.rotation = targetRotation;
            
            Debug.Log($"targetRotation: {targetRotation.eulerAngles}");
            
            this.IsOnWall = true;
        }
    }
}