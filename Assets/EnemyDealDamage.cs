using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{
    [SerializeField] private EnemyScript EnemyScript;

    private void OnTriggerEnter(Collider other)
    {
        this.EnemyScript.EnterDamageTrigger();
    }

    private void OnTriggerExit(Collider other)
    {
        this.EnemyScript.ExitDamageTrigger();
    }
}
