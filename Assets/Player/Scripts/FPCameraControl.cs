using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


namespace Player.Scripts
{
    public class FPCameraControl : MonoBehaviour, IViewMode
    {
        [SerializeField] public Transform Camera;

        private Vector2 RotationInput;

        [SerializeField] public float RotationSpeed = 5;

        [SerializeField] public Transform Player;

        public EViewMode ViewMode { get; set; }

        // Update is called once per frame
        void Update()
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;

            float mx = RotationInput.x * Time.deltaTime * this.RotationSpeed;
            float my = RotationInput.y * Time.deltaTime * this.RotationSpeed;

            Vector3 rotCamera = transform.rotation.eulerAngles + new Vector3(-my, 0, 0);
            Vector3 rotBody = Player.transform.rotation.eulerAngles + new Vector3(0, mx, 0);
            rotCamera.x = this.ClampAngle(rotCamera.x, -75f, 75f);

            transform.rotation = Quaternion.Euler(rotCamera);
            Player.transform.rotation = Quaternion.Euler(rotBody);
        }

        public void OnLook(InputAction.CallbackContext _context)
        {
            if (this.ViewMode != EViewMode.ThreeDimension) return;
            
            this.RotationInput = _context.ReadValue<Vector2>();
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