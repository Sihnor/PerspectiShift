using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    enum EPlayerState
    {
        StartDragging,
        Dragging,
        StopDragging,
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
        private Vector3 StartCameraPosition = Vector3.zero;

        private bool TargetCameraRotationReached = true;
        private Quaternion TargetCameraRotation = Quaternion.identity;
        private Quaternion StartCameraRotation = Quaternion.identity;

        private bool TargetPlayerRotationReached = true;
        private Quaternion TargetPlayerRotation = Quaternion.identity;

        // Input
        [SerializeField] private PlayerInput PlayerInput;
        private InputAction LookAction;
        private InputAction LookControllerAction;

        private EPlayerState PlayerState = EPlayerState.Normal;

        [SerializeField, Range(1, 1000)] private int InterpolationSpeed = 10;

        public EViewMode ViewMode { get; set; }

        private void Awake()
        {
            this.LookAction = this.PlayerInput.currentActionMap.FindAction("Look3D");
            this.LookControllerAction = this.PlayerInput.currentActionMap.FindAction("Look3DController");

            EventManager.Instance.FOnPlayDraggingAnimation += SetIsStartDragging;
        }

        private void Start()
        {
            this.LookAction.performed += context => Look(context, this.RotationSpeed);
            this.LookControllerAction.performed += context => Look(context, this.RotationControllerSpeed);

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
                case EPlayerState.StartDragging:
                    DraggingAnimation();
                    break;
                case EPlayerState.Dragging:
                    break;
                case EPlayerState.StopDragging:
                    DraggingAnimation();
                    break;
                case EPlayerState.Normal:
                    NormalState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Look(InputAction.CallbackContext context, float rotationSpeed)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;
            if (this.PlayerState != EPlayerState.Normal) return;

            this.RotationInput = context.ReadValue<Vector2>();

            this.RotationInput.x *= rotationSpeed;
        }

        private void EndLook(InputAction.CallbackContext context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;

            this.RotationInput = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// Regelt die PLayer States sowie die Target positions and rotationd der Kamera und des Spielers.
        /// </summary>
        /// <param name="startDragging"></param>
        private void SetIsStartDragging(bool startDragging)
        {
            if (this.PlayerState == EPlayerState.Normal)
            {
                this.StartCameraRotation = this.Camera.transform.rotation;
                this.StartCameraPosition = this.Camera.transform.position - this.Player.position;
            }
            
            if (startDragging)
            {
                this.PlayerState = EPlayerState.StartDragging;
                
                this.TargetPlayerRotation = Quaternion.LookRotation((this.Player.GetComponent<DragShadowObject>().GetGrabbedObject().normal * -1), this.Player.transform.up);
                this.TargetCameraPosition = this.Player.position + this.Camera.transform.TransformDirection(new Vector3(0.0f, 2.75f, -3.0f));
                this.TargetCameraRotation = this.StartCameraRotation * Quaternion.Euler(30, 0, 0);
            }
            
            if (!startDragging)
            {
                this.PlayerState = EPlayerState.StopDragging;
                //this.TargetCameraPosition = this.Player.position + new Vector3(0.0f, 0.85f, -0.18f);
                this.TargetCameraPosition = this.Player.position + this.StartCameraPosition;
                this.TargetCameraRotation = this.StartCameraRotation;
            }
            
            this.TargetPlayerRotationReached = false;
            this.TargetCameraPositionReached = false;
            this.TargetCameraRotationReached = false;
        }

        /// <summary>
        /// Eine State Machine die die Animation zum greifen und loslassen des Spielers regelt.
        /// </summary>
        private void DraggingAnimation()
        {
            bool isFinished1 = InterpolatePlayerRotation();
            //if (!isFinished1) return;
            bool isFinished2 = InterpolateCameraPosition();
            //if (!isFinished2) return;
            bool isFinished3 = InterpolateCameraRotation();

            if (isFinished1 && isFinished2 && isFinished3 && this.PlayerState == EPlayerState.StartDragging)
            {
                this.Player.transform.rotation = this.TargetPlayerRotation;
                this.Camera.transform.position = this.TargetCameraPosition;
                this.Camera.transform.rotation = this.TargetCameraRotation;
                this.PlayerState = EPlayerState.Dragging;
                EventManager.Instance.OnEndDraggingAnimation(true);
            }

            if (isFinished1 && isFinished2 && isFinished3 && this.PlayerState == EPlayerState.StopDragging)
            {
                this.Player.transform.rotation = this.TargetPlayerRotation;
                this.Camera.transform.position = this.TargetCameraPosition;
                this.Camera.transform.rotation = this.TargetCameraRotation;
                this.PlayerState = EPlayerState.Normal;
                EventManager.Instance.OnEndDraggingAnimation(false);
            }
        }

        /// <summary>
        /// Interpoliert den Spieler das er sich senkrecht zum objekt bewegt.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Interpoliert die Kamera das sie sich zu der Zielposition bewegt.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Interpoliert die Kamera das sie sich zu der Zielrotation bewegt.
        /// </summary>
        /// <returns></returns>
        private bool InterpolateCameraRotation()
        {
            if (this.TargetCameraRotationReached) return true;
            const float tolerance = 0.001f;

            Vector3 cameraRotation = this.Camera.transform.rotation.eulerAngles;
            Vector3 targetCameraRotation = this.TargetCameraRotation.eulerAngles;
            
            Vector3 interpolatedRotation = Vector3.Lerp(cameraRotation, targetCameraRotation, InterpolationSpeed * Time.deltaTime);
            this.Camera.transform.rotation = Quaternion.Euler(interpolatedRotation);

            if (Mathf.Abs(cameraRotation.x - targetCameraRotation.x) < tolerance &&
                Mathf.Abs(cameraRotation.y - targetCameraRotation.y) < tolerance &&
                Mathf.Abs(cameraRotation.z - targetCameraRotation.z) < tolerance)
            {
                this.TargetCameraRotationReached = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Der Normal State ist der normale Zustand des Spielers.
        /// Regelt die Yaw Rotations des Spielers und die Roll Rotation der Kamera.
        /// </summary>
        private void NormalState()
        {
            // Pitch Rotation
            float temp = this.TargetPlayerRotation.eulerAngles.y;
            temp += this.RotationInput.x;

            this.TargetCameraRotation.x -= this.RotationInput.y;
            this.TargetCameraRotation.x = Mathf.Clamp(this.TargetCameraRotation.x, -70, 70);

            // Yaw Rotation
            this.Player.rotation = Quaternion.Euler(this.TargetPlayerRotation.eulerAngles);
            this.TargetPlayerRotation = Quaternion.Euler(this.TargetPlayerRotation.eulerAngles.x, temp, this.TargetPlayerRotation.eulerAngles.z);

            // Camera Rotation
            transform.rotation = Quaternion.Euler(new Vector3(this.TargetCameraRotation.x, temp, 0));
        }
    }
}