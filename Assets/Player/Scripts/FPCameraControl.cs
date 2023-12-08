using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


namespace Player.Scripts
{
    public class FPCameraControl : MonoBehaviour, IViewMode
    {
        [SerializeField] private Transform Camera;

        private Vector2 RotationInput;

        [SerializeField] private float RotationSpeed = .1f;
        [SerializeField] private float RotationControllerSpeed = 10f;

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
            this.LookAction.performed += _context => Look(_context, this.RotationSpeed);
            this.LookControllerAction.performed += _context => Look(_context, this.RotationControllerSpeed);

            this.LookAction.canceled += EndLook;
            this.LookControllerAction.canceled += EndLook;
            
            this.ViewMode = EViewMode.ThreeDimension;
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;
            
            this.RotationVecCam.x -= this.RotationInput.y;
            this.RotationVecCam.x = Mathf.Clamp(this.RotationVecCam.x, -45, 45);
            this.RotationVecPlayer.y += this.RotationInput.x;
            
            this.Player.rotation = Quaternion.Euler(this.RotationVecPlayer);
            this.Camera.transform.rotation = Quaternion.Euler(new Vector3(this.RotationVecCam.x, this.RotationVecPlayer.y, 0));
        }

        private void Look(InputAction.CallbackContext _context, float _rotationSpeed)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;
            
            this.RotationInput = _context.ReadValue<Vector2>();
            this.RotationInput.x *= _rotationSpeed;
        }


        
        private void EndLook(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;
            
            this.RotationInput = _context.ReadValue<Vector2>();
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