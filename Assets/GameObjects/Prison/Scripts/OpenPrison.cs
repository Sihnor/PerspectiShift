using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPrison : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EventManager.Instance.OnPrisonOpen();
            GetComponent<BoxCollider>().enabled = false;
        }
        
    }
}

