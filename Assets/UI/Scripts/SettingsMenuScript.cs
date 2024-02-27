using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsMenuScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Exit via escape or B/Kreis button
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            SceneLoader.Instance.LoadSceneAsync(ESceneIndices.MainMenu, false);
        }
    }
}
