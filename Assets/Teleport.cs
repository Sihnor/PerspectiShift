using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] public Transform Platform1;
    [SerializeField] public Transform Platform2;
    
    [SerializeField] public int Cooldown = 5;

    private float TimeForNextTeleport;

    private void Start()
    {
        this.TimeForNextTeleport = 0;
    }

    public void CollisionDetected(Collision _collision, Platform.EPlatform _platform)
    {
        if (this.TimeForNextTeleport > Time.time)
        {
            return;
        }
        
        if (_platform == Platform.EPlatform.Platform1)
        {
            Vector3 temp = Platform2.position;
            temp.y += 5;
            _collision.gameObject.transform.position = temp;
        }

        if (_platform == Platform.EPlatform.Platform2)
        {
            Vector3 temp = Platform1.position;
            temp.y += 5;
            _collision.gameObject.transform.position = temp;
        }

        this.TimeForNextTeleport = Time.time + Cooldown;
    }
}
