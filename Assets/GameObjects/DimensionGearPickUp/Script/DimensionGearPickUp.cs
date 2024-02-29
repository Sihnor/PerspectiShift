using System;
using System.Collections;
using System.Collections.Generic;
using Player.Scripts;
using UnityEngine;

public class DimensionGearPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<DimensionGear>().CollectDimensionGear();
        }
    }
}
