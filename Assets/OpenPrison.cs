using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPrison : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EventManager.Instance.OnPrisonOpen();
        GetComponent<BoxCollider>().enabled = false;
    }
}

