using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player.Scripts
{
    public class TPCameraControl : MonoBehaviour, IViewMode
    {
        [SerializeField] public Transform Player;

        [SerializeField] private PlayerInput PlayerInput;

        private InputAction SwitchPlaneAction;

        public EViewMode ViewMode { get; set; }

        private void Awake()
        {
            this.SwitchPlaneAction = this.PlayerInput.currentActionMap.FindAction("SwitchPlane");
        }

        // Start is called before the first frame update
        void Start()
        {
            this.SwitchPlaneAction.started += SwitchPlane;

            this.ViewMode = EViewMode.ThreeDimension;
        }

        public void SwitchPlane(InputAction.CallbackContext _context)
        {
            if (this.ViewMode == EViewMode.ThreeDimension) return;

            this.Player.transform.Rotate(Vector3.up, 90 * _context.ReadValue<Vector2>().x);
        }
    }
}