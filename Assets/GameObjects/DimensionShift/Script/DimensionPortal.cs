using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DimensionProtal : MonoBehaviour
{
    [SerializeField] public Player.Scripts.CameraSelecter DimensionManager;

    [SerializeField] public int Cooldown = 5;

    private float TimeForNextSwitch;

    // Start is called before the first frame update
    void Start()
    {
        TimeForNextSwitch = 0;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (TimeForNextSwitch <= Time.time)
        {
            DimensionManager.OnDimensionSwitch();

            TimeForNextSwitch = Time.time + Cooldown;    
        }
        
    }
}
