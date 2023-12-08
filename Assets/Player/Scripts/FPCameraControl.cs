using UnityEngine;
using UnityEngine.InputSystem;


namespace Player.Scripts
{
    public class FPCameraControl : MonoBehaviour, IViewMode
    {
        [SerializeField] private Transform Camera;

        private Vector2 RotationInput;

        [SerializeField] private float RotationSpeed = .01f;
        [SerializeField] private float RotationSpeedController = 1f;

        [SerializeField] private Transform Player;

        [SerializeField] private PlayerInput PlayerInput;

        private InputAction LookAction;
        private InputAction LookControllerAction;


        private Vector3 RotationVecCam;

        private Vector3 RotationVecPlayer;

        public EViewMode ViewMode { get; set; }

        private void Awake()
        {
            this.LookAction = this.PlayerInput.currentActionMap.FindAction("Look3D");
            this.LookControllerAction = this.PlayerInput.currentActionMap.FindAction("Look3DController");
        }

        private void Start()
        {
            this.LookAction.performed += Look;
            this.LookControllerAction.performed += LookController;
            this.LookAction.canceled += EndLook;
            
            this.ViewMode = EViewMode.ThreeDimension;
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;
            
            this.Player.rotation = Quaternion.Euler(this.RotationVecPlayer);
            this.Camera.transform.rotation = Quaternion.Euler(new Vector3(this.RotationVecCam.x, this.RotationVecPlayer.y, 0));
        }

        private void Look(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;
            
            this.RotationInput = _context.ReadValue<Vector2>();
            
            this.RotationVecCam.x -= this.RotationInput.y;
            this.RotationVecCam.x = Mathf.Clamp(this.RotationVecCam.x, -45, 45);
            this.RotationVecPlayer.y += this.RotationInput.x * this.RotationSpeed;
        }

        private void LookController(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;
            
            this.RotationInput = _context.ReadValue<Vector2>();
            
            this.RotationVecCam.x -= this.RotationInput.y;
            this.RotationVecCam.x = Mathf.Clamp(this.RotationVecCam.x, -45, 45);
            this.RotationVecPlayer.y += this.RotationInput.x * this.RotationSpeedController;
        }
        
        private void EndLook(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;
            
            //this.RotationInput = _context.ReadValue<Vector2>();
        }

        private static float ClampAngle(float _angle, float _from, float _to)
        {
            // accepts e.g. -80, 80
            if (_angle < 0f) _angle = 360 + _angle;
            if (_angle > 180f) return Mathf.Max(_angle, 360 + _from);
            return Mathf.Min(_angle, _to);
        }
    }
}