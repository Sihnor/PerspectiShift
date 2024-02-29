using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prison : MonoBehaviour
{
    [SerializeField] private GameObject PrisonGlass;
    [SerializeField] private GameObject Enemy;

    private void Awake()
    {
        EventManager.Instance.FOnPrisonOpen += OpenPrisonGlass;
    }

    private void OpenPrisonGlass()
    {
        this.PrisonGlass.SetActive(false);
        this.Enemy.SetActive(true);
    }
}
