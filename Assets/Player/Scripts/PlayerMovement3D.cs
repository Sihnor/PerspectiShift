using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class PlayerMovement3D : Movement
    {
        public override EViewMode ViewMode
        {
            set
            {
                if (value == EViewMode.TwoDimension)
                {
                    this.RotationBeforeSwitch = transform.rotation;
                }

                if (value == EViewMode.ThreeDimension)
                {
                    transform.rotation = RotationBeforeSwitch;
                }

                base.ViewMode = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            this.MoveAction = this.PlayerInput.currentActionMap.FindAction("Move3D");
            this.JumpAction = this.PlayerInput.currentActionMap.FindAction("Jump3D");
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            this.MoveAction.performed += Move;
            this.JumpAction.started += Jump;
            this.MoveAction.canceled += EndMove;
            this.JumpAction.canceled += EndJump;
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.ViewMode != Scripts.EViewMode.ThreeDimension) return;

            SpeedControl();

            SetDragFactor();
        }

        private void FixedUpdate()
        {
            if (this.ViewMode != Scripts.EViewMode.ThreeDimension) return;

            float scalar = this.MoveSpeed * (1 / Time.deltaTime);
            Vector3 velocity = this.MovementInput.y * scalar * transform.forward + this.MovementInput.x * scalar * transform.right;
            velocity.y = this.Rigidbody.velocity.y;

            if (!this.IsGrounded) velocity.x *= 0.1f;
            if (!this.IsGrounded) velocity.z *= 0.1f;

            this.Rigidbody.AddForce(velocity);
        }

        public override void Move(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != Scripts.EViewMode.ThreeDimension) return;

            this.MovementInput = _context.ReadValue<Vector2>();
        }

        public override void EndMove(InputAction.CallbackContext _context)
        {
            this.MovementInput = _context.ReadValue<Vector2>();
        }

        public override void Jump(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != Scripts.EViewMode.ThreeDimension) return;

            this.Rigidbody.AddForce(new Vector3(0, this.JumpSpeed, 0), ForceMode.Impulse);
        }

        public override void EndJump(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != Scripts.EViewMode.ThreeDimension) return;

            this.Rigidbody.AddForce(new Vector3(0, -this.JumpSpeed, 0), ForceMode.Impulse);
        }

        protected override void SpeedControl()
        {
            if (this.ViewMode != Scripts.EViewMode.ThreeDimension) return;

            Vector3 velocity = this.Rigidbody.velocity;
            Vector3 rigidbodyVelocity = new Vector3(velocity.x, 0, velocity.z);

            if (rigidbodyVelocity.magnitude > this.MoveSpeed)
            {
                Vector3 velocityNormalised = rigidbodyVelocity.normalized;
                velocityNormalised *= this.MoveSpeed;

                this.Rigidbody.velocity = new Vector3(velocityNormalised.x, this.Rigidbody.velocity.y, velocityNormalised.z);
            }
        }

        private void SetDragFactor()
        {
            if (this.ViewMode != Scripts.EViewMode.ThreeDimension) return;

            this.IsGrounded = Physics.Raycast(transform.position, Vector3.down, this.PlayerCollider.bounds.extents.y + 0.2f, this.Ground);

            if (this.IsGrounded)
            {
                this.Rigidbody.drag = this.DragForce;
            }
            else
            {
                this.Rigidbody.drag = 0;
            }
        }
    }
}