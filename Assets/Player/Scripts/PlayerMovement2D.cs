using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class PlayerMovement2D : Movement
    {
        [SerializeField] private GameObject SpritePlayer;
        
        public override EViewMode ViewMode
        {
            set
            {
                if (value == EViewMode.ThreeDimension)
                {
                    this.RotationBeforeSwitch = transform.rotation;
                }

                if (value == EViewMode.TwoDimension)
                {
                    transform.rotation = this.RotationBeforeSwitch;
                }

                base.ViewMode = value;
            }
        }
        
        private bool UsedDoubleJump = false;

        protected override void Awake()
        {
            base.Awake();
            
            this.MoveAction = this.PlayerInput.currentActionMap.FindAction("Move2D");
            this.JumpAction = this.PlayerInput.currentActionMap.FindAction("Jump2D");
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            this.MoveAction.performed += Move;
            this.JumpAction.performed += Jump;
            this.MoveAction.canceled += EndMove;
            this.JumpAction.canceled += EndJump;
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.ViewMode != Scripts.EViewMode.TwoDimension) return;
            
            SpeedControl();
            
            SetDragFactor();
        }
    
        private void FixedUpdate()
        {
            if (this.ViewMode != Scripts.EViewMode.TwoDimension) return;
            
            Vector3 velocity = this.SpritePlayer.transform.right * (this.MovementInput.x * this.MoveSpeed * (1 / Time.deltaTime));
            velocity.y = this.Rigidbody.velocity.y;
            
            if (!this.IsGrounded)
            {
                velocity.x *= 0.1f;
                velocity.z *= 0.1f;
            }
            
            this.Rigidbody.AddForce(velocity);
        }

        public override void Move(InputAction.CallbackContext context)
        {
            if (this.ViewMode != Scripts.EViewMode.TwoDimension) return;

            this.MovementInput = context.ReadValue<Vector2>();
        }

        public override void EndMove(InputAction.CallbackContext context)
        {
            this.MovementInput = context.ReadValue<Vector2>();
        }
        
        public override void Jump(InputAction.CallbackContext context)
        {
            if (this.ViewMode != Scripts.EViewMode.TwoDimension) return;
            if (!this.IsGrounded && this.UsedDoubleJump) return;

            if (context.performed)
            {
                if (this.IsGrounded)
                {
                    this.Rigidbody.AddForce(new Vector3(0, this.JumpSpeed, 0), ForceMode.Impulse);
                }
                else
                {
                    this.Rigidbody.velocity = new Vector3(this.Rigidbody.velocity.x, 0, this.Rigidbody.velocity.z);
                    this.Rigidbody.AddForce(new Vector3(0, this.JumpSpeed * .7f, 0), ForceMode.Impulse);
                    this.UsedDoubleJump = true;
                }
            }
        }

        public override void EndJump(InputAction.CallbackContext context)
        {
            if (this.ViewMode != Scripts.EViewMode.TwoDimension) return;
            
            Vector3 velocity = this.Rigidbody.velocity;
            velocity = new Vector3(velocity.x, velocity.y * 0.5f, velocity.z);
            this.Rigidbody.velocity = velocity;
        }

        protected override void SpeedControl()
        {
            if (this.ViewMode != Scripts.EViewMode.TwoDimension) return;
            
            
            Vector3 rigidbodyVelocity = new Vector3(this.Rigidbody.velocity.x, 0, this.Rigidbody.velocity.z);

            if (rigidbodyVelocity.magnitude > this.MoveSpeed)
            {
                Vector3 velocityNormalised = rigidbodyVelocity.normalized;
                velocityNormalised *= this.MoveSpeed;
                this.Rigidbody.velocity = new Vector3(velocityNormalised.x, this.Rigidbody.velocity.y, velocityNormalised.z);
            }
        }
        
        private void SetDragFactor()
        {
            if (this.ViewMode != Scripts.EViewMode.TwoDimension) return;
            
            this.IsGrounded = Physics.Raycast(transform.position + new Vector3(0,0.1f,0), Vector3.down, this.PlayerCollider.bounds.extents.y + 0.2f, this.GroundLayerMask);

            if (this.IsGrounded)
            {
                this.Rigidbody.drag = this.DragForce;
                this.UsedDoubleJump = false;
            }
            else
            {
                this.Rigidbody.drag = 0;
            }
        }
        
    }
}
