using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public abstract class Movement : MonoBehaviour, IViewMode
    {
        [Header("Movement")]
        public float MoveSpeed = 5.0f;
        protected Vector2 MovementInput;
        
        [Header("Jump"), SerializeField] 
        public float JumpSpeed = 7.0f;
        
        [Header("Grounded"), SerializeField]
        public float DragForce = 7;
        protected LayerMask GroundLayerMask;

        protected Quaternion RotationBeforeSwitch;
        public virtual EViewMode ViewMode{ get; set;}

        protected bool IsGrounded;
        
        protected Collider PlayerCollider;
        protected Rigidbody Rigidbody;
        
        protected PlayerInput PlayerInput;
        protected InputAction MoveAction;
        protected InputAction JumpAction;

        protected virtual void Awake()
        {
            this.PlayerInput = GetComponent<PlayerInput>();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            this.Rigidbody = GetComponent<Rigidbody>();
            this.PlayerCollider = GetComponentInChildren<MeshCollider>();
            
            Cursor.lockState = CursorLockMode.Locked;
            this.ViewMode = Scripts.EViewMode.ThreeDimension;

            if (this.Rigidbody)
            {
                this.Rigidbody.freezeRotation = true;
            }
        }

        public abstract void Move(InputAction.CallbackContext context);
        public abstract void EndMove(InputAction.CallbackContext context);
        
        public abstract void Jump(InputAction.CallbackContext context);
        public abstract void EndJump(InputAction.CallbackContext context);
        
        protected abstract void SpeedControl();
    }
}
