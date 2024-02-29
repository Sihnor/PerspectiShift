using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBody : MonoBehaviour
{
    [SerializeField] private GameObject SpriteBody;
    [SerializeField] private GameObject ModelBody;
    
    private void Awake()
    {
        EventManager.Instance.FOnDimensionSwitch += OnDimensionSwitch;
    }

    private void OnDimensionSwitch()
    {
        this.ModelBody.SetActive(this.SpriteBody.activeSelf);
        this.SpriteBody.SetActive(!this.SpriteBody.activeSelf);
    }
}
