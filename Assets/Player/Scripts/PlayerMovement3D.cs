using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class PlayerMovement3D : Movement
    {
        private Vector2 RotationInput = Vector2.zero;

        [SerializeField] public float RotationSpeed = 5;
        
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;

            SpeedControl();

            SetDragFactor();

            float mx = RotationInput.x * Time.deltaTime * this.RotationSpeed;
            float my = RotationInput.y * Time.deltaTime * this.RotationSpeed;

            Vector3 rot = transform.rotation.eulerAngles + new Vector3(-my, mx, 0);
            rot.x = this.ClampAngle(rot.x, -75f, 75f);

            transform.eulerAngles = rot;
        }

        private void FixedUpdate()
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;

            float scalar = this.MoveSpeed * (1 / Time.deltaTime);
            Vector3 velocity = this.MovementInput.y * scalar * transform.forward + this.MovementInput.x * scalar * transform.right;
            velocity.y = this.Rigidbody.velocity.y;
            
            if (!this.IsGrounded) velocity.x *= 0.1f;
            if (!this.IsGrounded) velocity.z *= 0.1f;
            
            this.Rigidbody.AddForce(velocity);
        }

        public override void OnMove(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;

            this.MovementInput = _context.ReadValue<Vector2>();
        }

        public override void OnJump(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;

            if (_context.performed)
            {
                this.Rigidbody.AddForce(new Vector3(0, this.JumpSpeed, 0), ForceMode.Impulse);
            }

            if (_context.canceled)
            {
                this.Rigidbody.AddForce(new Vector3(0, -this.JumpSpeed, 0), ForceMode.Impulse);
            }
        }

        public void OnLook(InputAction.CallbackContext _context)
        {
            this.RotationInput = _context.ReadValue<Vector2>();
        }

        protected override void SpeedControl()
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;

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
            if (this.ViewMode != EViewMode.ThreeDimension) return;

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

        float ClampAngle(float _angle, float _from, float _to)
        {
            // accepts e.g. -80, 80
            if (_angle < 0f) _angle = 360 + _angle;
            if (_angle > 180f) return Mathf.Max(_angle, 360 + _from);
            return Mathf.Min(_angle, _to);
        }
    }
}