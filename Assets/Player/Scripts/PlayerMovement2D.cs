using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class PlayerMovement2D : Movement
    {
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.ViewMode != EViewMode.TwoDimension) return;
            
            SpeedControl();
            
            SetDragFactor();
        }
    
        private void FixedUpdate()
        {
            if (this.ViewMode != EViewMode.TwoDimension) return;
            
            
            Vector3 velocity = new Vector3(this.MovementInput.x * this.MoveSpeed * (1/Time.deltaTime), this.Rigidbody.velocity.y ,0);
            
            if (!this.IsGrounded)
            {
                velocity.x *= 0.1f;
                
            }
            
            this.Rigidbody.AddForce(velocity);
        }

        public override void OnMove(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.TwoDimension) return;

            Vector2 contextVector = _context.ReadValue<Vector2>();

            this.MovementInput = contextVector;
        }

        public override void OnJump(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.TwoDimension) return;
            
            if (_context.performed)
            {
                this.Rigidbody.AddForce(new Vector3(0,this.JumpSpeed,0), ForceMode.Impulse);
            }
            
            // Cancel the jump 
            if (!_context.canceled) return;
            
            //this.Rigidbody.AddForce(new Vector3(0,-this.JumpSpeed,0), ForceMode.Impulse);
            Vector3 velocity = this.Rigidbody.velocity;
            velocity = new Vector3(velocity.x, velocity.y * 0.5f, velocity.z);
            this.Rigidbody.velocity = velocity;
        }

        protected override void SpeedControl()
        {
            if (this.ViewMode != EViewMode.TwoDimension) return;
            
            
            Vector3 rigidbodyVelocity = new Vector3(this.Rigidbody.velocity.x, 0, 0);

            if (rigidbodyVelocity.magnitude > this.MoveSpeed)
            {
                Vector3 velocityNormalised = rigidbodyVelocity.normalized;
                velocityNormalised *= this.MoveSpeed;

                this.Rigidbody.velocity = new Vector3(velocityNormalised.x, this.Rigidbody.velocity.y, velocityNormalised.z);
            }
        }
        
        private void SetDragFactor()
        {
            if (this.ViewMode != EViewMode.TwoDimension) return;
            
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
