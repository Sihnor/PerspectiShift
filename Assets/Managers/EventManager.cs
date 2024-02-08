using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    
    public event Action<bool> FOnDimensionGearAvailable; 
    public event Action<bool> FOnDimensionGearPosses;
    public event Action<bool> FOnDimensionSwitch; 

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
        FOnDimensionSwitch?.Invoke(true);
    }
}
