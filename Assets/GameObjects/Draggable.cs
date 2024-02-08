using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    bool IsDraggable = false;
    
    private void OnTriggerEnter(Collider other)
    {
        this.IsDraggable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        this.IsDraggable = false;
    }
    
    public bool GetIsDraggable()
    {
        return this.IsDraggable;
    }
}
