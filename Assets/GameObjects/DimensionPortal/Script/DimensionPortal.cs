using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DimensionProtal : MonoBehaviour
{
    [SerializeField] private int Cooldown = 5;
    [SerializeField] private GameObject TeleportPoint;
    [SerializeField] private bool SingleUse;
    private bool IsUsed = false;

    private float TimeForNextSwitch = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (!(this.TimeForNextSwitch <= Time.time)) return;
        if (this.IsUsed) return;
        
        
        if (this.SingleUse) this.IsUsed = true;
        
        
        
        EventManager.Instance.OnDimensionSwitch();

        this.TimeForNextSwitch = Time.time + this.Cooldown;

        return;
        if (this.TeleportPoint != null)
        {
            other.gameObject.transform.position = this.TeleportPoint.transform.position;
        }
        else
        {
            other.gameObject.transform.position = this.transform.position + new Vector3(0,0,0.5f);
        }
    }
}
