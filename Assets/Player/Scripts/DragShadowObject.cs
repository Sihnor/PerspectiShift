using Player.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragShadowObject : MonoBehaviour
{
    private PlayerInput PlayerInput;
    private InputAction DragAction;
    private bool IsDragging;

    private RaycastHit Hit;
    private Vector3 DragOffset = Vector3.zero;
    
    private void Awake()
    {
        this.PlayerInput = GetComponent<PlayerInput>();
        this.DragAction = this.PlayerInput.currentActionMap.FindAction("DragShadowObject");
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

    private void DragObject(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(transform.position + transform.up, transform.forward, out this.Hit, 10f, LayerMask.GetMask("ShadowObject")) 
            && this.Hit.collider.gameObject.CompareTag($"Draggable"))
        {
            if (this.Hit.collider.gameObject.GetComponent<Draggable>().GetIsDraggable() == false) return;
            
            this.DragOffset = this.Hit.collider.transform.position - transform.position;
            this.DragOffset -= this.Hit.normal * 0.02f;
            this.IsDragging = true;

            EventManager.Instance.OnPlayDraggingAnimation(true);
        }
    }
    
    private void StopDragObject(InputAction.CallbackContext context)
    {
        if (!this.IsDragging) return;

        this.IsDragging = false;
        EventManager.Instance.OnPlayDraggingAnimation(false);
    }
    
    public RaycastHit GetGrabbedObject()
    {
        return this.Hit;
    }
}