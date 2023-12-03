using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    public float LightBeamRange = 0;
    public float LightSpotAngle = 0;
    public bool IsLightOn = false;

    private Light TempLight;

    //[SerializeField] private Transform StartRayPosition;
    private Vector3 StartRayPosition;

    // Start is called before the first frame update
    private void Awake()
    {
        this.TempLight = GetComponentInChildren<Light>();
        this.StartRayPosition = transform.GetChild(1).position;
        this.LightBeamRange = this.TempLight.range;
        this.LightSpotAngle = this.TempLight.spotAngle;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (this.IsLightOn) RaycastFunction();
    }

    private void RaycastFunction()
    {
        RaycastHit[] raycastHit = Physics.RaycastAll(this.StartRayPosition, transform.forward, this.LightBeamRange, 
            ((1 << LayerMask.NameToLayer("ShadowWall")) | (1 << LayerMask.NameToLayer("ShadowObject"))));
        
        if (!CheckIfWall(raycastHit)) return;

        RaycastHit? objectHit = null;
        
        foreach (RaycastHit hit in raycastHit)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ShadowObject")) objectHit = hit;
        }

        if (objectHit == null) return;
        
    }
    
    private bool CheckIfWall(RaycastHit[] _raycastHit)
    {
        bool isThereAWall = false;

        foreach (RaycastHit hit in _raycastHit)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ShadowWall")) isThereAWall = true;

        }

        return isThereAWall;
    }
    
    
}