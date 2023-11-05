using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player.Scripts
{
    public class TPCameraControl : MonoBehaviour, IViewMode
    {
        public EViewMode ViewMode { get; set; }

        [SerializeField] public Transform Player;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnRotatePlane(InputAction.CallbackContext _context)
        {
            if (!_context.performed || this.ViewMode == EViewMode.ThreeDimension)
            {
                return;
            }

            Player.transform.Rotate(Vector3.up, 90 * _context.ReadValue<Vector2>().x);
        }
    }
}