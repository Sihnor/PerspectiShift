using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    enum EPlayerState
    {
        Dragging,
        Normal
    }

    public class CameraControl3D : MonoBehaviour, IViewMode
    {
        [SerializeField] private Transform Camera;
        [SerializeField] private Transform Player;

        // Rotation
        [SerializeField] private float RotationSpeed = .1f;
        [SerializeField] private float RotationControllerSpeed = 10f;
        private Vector2 RotationInput;

        // Interpolation
        private bool TargetCameraPositionReached = true;
        private Vector3 TargetCameraPosition = Vector3.zero;

        private bool TargetCameraRotationReached = true;
        private Quaternion TargetCameraRotation = Quaternion.identity;

        private bool TargetPlayerRotationReached = true;
        private Quaternion TargetPlayerRotation = Quaternion.identity;

        // Input
        [SerializeField] private PlayerInput PlayerInput;
        private InputAction LookAction;
        private InputAction LookControllerAction;

        private EPlayerState PlayerState = EPlayerState.Normal;

        public bool IsDragging = false;

        [SerializeField, Range(1, 1000)] private int InterpolationSpeed = 10;

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

            switch (this.PlayerState)
            {
                case EPlayerState.Dragging:
                    DraggingState();
                    break;
                case EPlayerState.Normal:
                    NormalState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Look(InputAction.CallbackContext _context, float _rotationSpeed)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;
            if (this.PlayerState == EPlayerState.Dragging) return;

            this.RotationInput = _context.ReadValue<Vector2>();

            this.RotationInput.x *= _rotationSpeed;
        }

        private void EndLook(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;

            this.RotationInput = _context.ReadValue<Vector2>();
        }

        public void SetIsDragging(bool _isDragging)
        {
            this.IsDragging = _isDragging;

            this.TargetCameraPositionReached = false;
            this.TargetCameraRotationReached = false;

            if (this.IsDragging)
            {
                this.PlayerState = EPlayerState.Dragging;
                this.TargetPlayerRotationReached = false;
                this.TargetPlayerRotation = Quaternion.LookRotation((this.Player.GetComponent<DragShadowObject>().GetGrabbedObject().normal * -1),
                    this.Player.transform.up);
                this.TargetCameraPosition = transform.position + this.Camera.transform.TransformDirection(new Vector3(0.0f, 0.75f, -3.0f));
                this.TargetCameraRotation = this.Camera.transform.rotation * Quaternion.Euler(15, 0, 0);

            }

            if (!this.IsDragging)
            {
                this.TargetCameraPosition = transform.position + new Vector3(0.0f, 0.0f, 0.0f);
                this.TargetCameraRotation = this.Camera.transform.rotation * Quaternion.Euler(-15.0f, 0.0f, 0.0f);
            }
        }

        private void DraggingState()
        {
            bool isFinished3 = InterpolatePlayerRotation();
            bool isFinished1 = InterpolateCameraPosition();
            bool isFinished2 = InterpolateCameraRotation();

            if (isFinished1 && isFinished2 && isFinished3 && !this.IsDragging)
            {
                this.PlayerState = EPlayerState.Normal;
            }
        }

        private void NormalState()
        {
            // Pitch Rotation
            float temp = this.TargetPlayerRotation.eulerAngles.y;
            temp += this.RotationInput.x;

            this.TargetCameraRotation.x -= this.RotationInput.y;
            this.TargetCameraRotation.x = Mathf.Clamp(this.TargetCameraRotation.x, -45, 45);

            // Yaw Rotation
            this.Player.rotation = Quaternion.Euler(this.TargetPlayerRotation.eulerAngles);
            this.TargetPlayerRotation = Quaternion.Euler(this.TargetPlayerRotation.eulerAngles.x, temp, this.TargetPlayerRotation.eulerAngles.z);

            // Camera Rotation
            transform.rotation = Quaternion.Euler(new Vector3(this.TargetCameraRotation.x, temp, 0));
        }

        private bool InterpolateCameraPosition()
        {
            if (this.TargetCameraPositionReached) return true;
            const float tolerance = 0.01f;

            this.Camera.transform.position = Vector3.Lerp(this.Camera.transform.position, this.TargetCameraPosition, InterpolationSpeed * Time.deltaTime);

            if (Mathf.Abs(this.TargetCameraPosition.x - this.Camera.transform.position.x) < tolerance &&
                Mathf.Abs(this.TargetCameraPosition.y - this.Camera.transform.position.y) < tolerance &&
                Mathf.Abs(this.TargetCameraPosition.z - this.Camera.transform.position.z) < tolerance)
            {
                this.TargetCameraPositionReached = true;
                return true;
            }

            return false;
        }

        private bool InterpolateCameraRotation()
        {
            if (this.TargetCameraRotationReached) return true;
            const float tolerance = 0.001f;

            Quaternion temp = Quaternion.Lerp(this.Camera.transform.rotation, this.TargetCameraRotation, InterpolationSpeed * Time.deltaTime);
            this.Camera.transform.rotation = temp;

            if (Mathf.Abs(Quaternion.Dot(this.Camera.transform.rotation, this.TargetCameraRotation)) > tolerance)
            {
                this.TargetCameraRotationReached = true;
                return true;
            }

            //if (Mathf.Abs(this.TargetCameraRotation.eulerAngles.x - this.Camera.transform.localRotation.eulerAngles.x) < tolerance &&
            //    Mathf.Abs(this.TargetCameraRotation.eulerAngles.y - this.Camera.transform.localRotation.eulerAngles.y) < tolerance &&
            //    Mathf.Abs(this.TargetCameraRotation.eulerAngles.z - this.Camera.transform.localRotation.eulerAngles.z) < tolerance)
            //{
            //    this.TargetCameraRotationReached = true;
            //    return true;
            //}

            return false;
        }

        private bool InterpolatePlayerRotation()
        {
            if (this.TargetPlayerRotationReached) return true;
            const float tolerance = 0.01f;

            Quaternion temp = Quaternion.Lerp(this.Player.transform.rotation, this.TargetPlayerRotation, InterpolationSpeed * Time.deltaTime);
            this.Player.transform.rotation = temp;

            if (Mathf.Abs(this.TargetPlayerRotation.eulerAngles.x - this.Player.transform.rotation.eulerAngles.x) < tolerance &&
                Mathf.Abs(this.TargetPlayerRotation.eulerAngles.y - this.Player.transform.rotation.eulerAngles.y) < tolerance &&
                Mathf.Abs(this.TargetPlayerRotation.eulerAngles.z - this.Player.transform.rotation.eulerAngles.z) < tolerance)
            {
                this.TargetPlayerRotationReached = true;
                return true;
            }

            return false;
        }
    }
}