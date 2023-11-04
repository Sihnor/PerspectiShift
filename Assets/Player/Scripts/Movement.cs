using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public abstract class Movement : MonoBehaviour
    {
        public enum EViewMode
        {
            TwoDimension,
            ThreeDimension
        }
        
        [Header("Movement")]
        public float MoveSpeed = 5.0f;
        protected Vector2 MovementInput;
        
        [Header("Jump"), SerializeField] 
        public float JumpSpeed = 7.0f;
        
        [Header("Grounded"), SerializeField]
        public float DragForce = 7;
        public LayerMask Ground;
        
        protected EViewMode ViewMode;
        protected bool IsGrounded;
        
        [SerializeField] public Collider PlayerCollider;
        protected Rigidbody Rigidbody;

        
        
        // Start is called before the first frame update
        public virtual void Start()
        {
            this.Rigidbody = GetComponent<Rigidbody>();
            
            Cursor.lockState = CursorLockMode.Locked;
            this.ViewMode = EViewMode.TwoDimension;

            if (this.Rigidbody)
            {
                this.Rigidbody.freezeRotation = true;
            }
        }

        public abstract void OnMove(InputAction.CallbackContext _context);
        
        public abstract void OnJump(InputAction.CallbackContext _context);
        
        protected abstract void SpeedControl();

        public void SetViewMode(EViewMode _newMode)
        {
            this.ViewMode = _newMode;
        }
    }
}
