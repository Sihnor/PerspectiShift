using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class PlayerMovement3D : Movement
    {
        // Start is called before the first frame update
        public override void Start()
        {
            this.ViewMode = EViewMode.ThreeDimension;
            
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.ViewMode != EViewMode.ThreeDimension)
            {
                return;
            }
            
            SpeedControl();
            
            SetDragFactor();
        }

        private void FixedUpdate()
        {
            if (this.ViewMode != EViewMode.ThreeDimension)
            {
                return;
            }
            
            Vector3 velocity = new Vector3(this.MovementInput.y * this.MoveSpeed * (1/Time.deltaTime), this.Rigidbody.velocity.y ,this.MovementInput.x * this.MoveSpeed * (1/Time.deltaTime) * -1);
            this.Rigidbody.AddForce(velocity);
        }

        public override void OnMove(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension)
            {
                return;
            }
            
            this.MovementInput = _context.ReadValue<Vector2>();
        }

        public override void OnJump(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension)
            {
                return;
            }
            
            if (_context.performed)
            {
                this.Rigidbody.AddForce(new Vector3(0,this.JumpSpeed,0), ForceMode.Impulse);
            }
            
            if(_context.canceled)
            {
                this.Rigidbody.AddForce(new Vector3(0,-this.JumpSpeed,0), ForceMode.Impulse);
            }
        }

        protected override void SpeedControl()
        {
            if (this.ViewMode != EViewMode.ThreeDimension)
            {
                return;
            }
            
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
            if (this.ViewMode != EViewMode.ThreeDimension)
            {
                return;
            }
            
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
