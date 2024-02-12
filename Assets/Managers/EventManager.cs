using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    
    public event Action<bool> FOnDimensionGearAvailable; 
    public event Action<bool> FOnDimensionGearPosses;
    public event Action FOnDimensionSwitch; 
    public event Action<bool> FOnPlayDraggingAnimation;
    public event Action<bool> FOnEndDraggingAnimation;
    public event Action FOnDamagePlayer;
    public event Action FOnPrisonOpen;
    public event Action FOnPlayerDeath;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnDimensionGearAvailable(bool isAvailable)
    {
        FOnDimensionGearAvailable?.Invoke(isAvailable);
    }
    
    public void OnDimensionGearPosses(bool isPosses)
    {
        GameManager.Instance.HasDimensionGear = isPosses;
        FOnDimensionGearPosses?.Invoke(isPosses);
    }
    
    public void OnDimensionSwitch()
    {
        FOnDimensionSwitch?.Invoke();
    }
    
    public void OnPlayDraggingAnimation(bool startDraggingAnimation)
    {
        FOnPlayDraggingAnimation?.Invoke(startDraggingAnimation);
    }
    
    public void OnEndDraggingAnimation(bool startDragging)
    {
        FOnEndDraggingAnimation?.Invoke(startDragging);
    }
    
    public void OnPrisonOpen()
    {
        FOnPrisonOpen?.Invoke();
    }
    
    public void OnDamagePlayer()
    {
        FOnDamagePlayer?.Invoke();
    }
    
    public void OnPlayerDeath()
    {
        FOnPlayerDeath?.Invoke();
    }
}
