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
        void Start()
        {
            this.SwitchPlaneAction.started += SwitchPlane;

            this.ViewMode = EViewMode.ThreeDimension;
        }

        public void SwitchPlane(InputAction.CallbackContext _context)
        {
            if (this.ViewMode == EViewMode.ThreeDimension) return;
            if (this.IsOnWall) return;

            this.Player.transform.Rotate(Vector3.up, 90 * _context.ReadValue<Vector2>().x);
        }

        public void SetRotationToWall(Vector3 _normal)
        {
            Quaternion targetRotation = Quaternion.LookRotation( (_normal * -1), this.Player.transform.up);

            this.Player.transform.rotation = targetRotation;
            
            this.IsOnWall = true;
        }
    }
}