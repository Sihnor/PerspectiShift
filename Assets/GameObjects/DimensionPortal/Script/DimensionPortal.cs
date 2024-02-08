using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DimensionProtal : MonoBehaviour
{
    [SerializeField] private int Cooldown = 5;

    private float TimeForNextSwitch = 0;
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (!(this.TimeForNextSwitch <= Time.time)) return;
        
        EventManager.Instance.OnDimensionSwitch();

        this.TimeForNextSwitch = Time.time + this.Cooldown;  
        other.gameObject.transform.position = this.transform.position + new Vector3(0,0,0.5f);
    }
}
