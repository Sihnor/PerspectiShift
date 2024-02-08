using Player.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragShadowObject : MonoBehaviour
{
    private PlayerInput PlayerInput;
    private InputAction DragAction;
    private bool IsDragging = false;

    private RaycastHit Hit;
    private Vector3 DragOffset = Vector3.zero;

    private PlayerMovement3D PlayerMovement3D;
    private CameraControl3D CameraControl3D;
    
    private void Awake()
    {
        this.PlayerInput = GetComponent<PlayerInput>();
        this.DragAction = this.PlayerInput.currentActionMap.FindAction("DragShadowObject");
        
        this.PlayerMovement3D = GetComponent<PlayerMovement3D>();
        this.CameraControl3D = GetComponentInChildren<CameraControl3D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.DragAction.started += DragObject;
        this.DragAction.canceled += StopDragObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsDragging)
        {
            this.Hit.collider.transform.position = transform.position + this.DragOffset;
        }
    }

    private void DragObject(InputAction.CallbackContext _context)
    {
        if (Physics.Raycast(transform.position, transform.forward, out this.Hit, 10f, LayerMask.GetMask("ShadowObject")) 
            && this.Hit.collider.gameObject.CompareTag($"Draggable"))
        {
            if (this.Hit.collider.gameObject.GetComponent<Draggable>().GetIsDraggable() == false) return;
            
            this.DragOffset = this.Hit.collider.transform.position - transform.position;
            this.DragOffset -= this.Hit.normal * 0.02f;
            this.IsDragging = true;

            this.PlayerMovement3D.SetIsDragging(true);
            this.CameraControl3D.SetIsDragging(true);
        }
        //else
        //{
        //    this.IsDragging = false;
        //    this.PlayerMovement3D.SetIsDragging(false);
        //    this.CameraControl3D.SetIsDragging(false);
        //}
    }
    
    private void StopDragObject(InputAction.CallbackContext _context)
    {
        if (!this.IsDragging) return;

        this.IsDragging = false;
        this.PlayerMovement3D.SetIsDragging(false);
        this.CameraControl3D.SetIsDragging(false);
    }
    
    public bool IsDragged()
    {
        return this.IsDragging;
    }
    
    public RaycastHit GetGrabbedObject()
    {
        return this.Hit;
    }
}