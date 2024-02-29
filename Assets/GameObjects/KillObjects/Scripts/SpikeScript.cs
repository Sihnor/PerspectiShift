using System;
using System.Collections;
using System.Collections.Generic;
using Player.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private Transform SpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent.transform.position = this.SpawnPoint.position;
            
            if (other.gameObject.GetComponentInParent<Player.Scripts.Movement>().ViewMode == EViewMode.TwoDimension)
            {
                EventManager.Instance.OnDimensionSwitch();
            }
        }
    }
}
