using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private BoxCollider ReceiveTrigger;
    [SerializeField] private BoxCollider DamageTrigger;
    [SerializeField] private Transform PlayerPosition;

    private bool HitPlayer = false;
    private bool IsActivated = false;
    [SerializeField, Range(0.1f, 5)] private float Speed = 5f;
    

    private void Awake()
    {
        EventManager.Instance.FOnPrisonOpen += OnOpenPrison;
        EventManager.Instance.FOnDimensionSwitch += OnDimensionSwitch;
        this.ReceiveTrigger.enabled = false;
        this.DamageTrigger.enabled = false;
    }

    private void Update()
    {
        if(!this.IsActivated) return;
        
        if (!this.HitPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, this.PlayerPosition.position, Speed * Time.deltaTime);
        }
    }

    public void EnterDamageTrigger()
    {
        this.ReceiveTrigger.enabled = false;
        EventManager.Instance.OnDamagePlayer();
        this.HitPlayer = true;
        
        Invoke(nameof(WaitTillAttackAgain), 1.5f);
    }

    private void WaitTillAttackAgain()
    {
        this.HitPlayer = false;
    }

    public void ExitDamageTrigger()
    {
        // Only in 2D Space
        if (this.Renderer.enabled)
        {
            this.ReceiveTrigger.enabled = true;    
        }
    }

    public void KillEnemy()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDimensionSwitch()
    {
        this.Renderer.enabled = !this.Renderer.enabled;
        this.ReceiveTrigger.enabled = !this.ReceiveTrigger.enabled;
    }

    private void OnOpenPrison()
    {
        this.Renderer.enabled = false;
        this.gameObject.transform.position = new Vector3(-14.176f, 0.3f, 47.089f);
        this.DamageTrigger.enabled = true;
        this.IsActivated = true;
    }
}