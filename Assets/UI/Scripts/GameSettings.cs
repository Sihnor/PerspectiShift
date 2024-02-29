using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings", order = 0)]
public class GameSettings : ScriptableObject
{
    [SerializeField] public float MasterVolume;
    [SerializeField] public float MusicVolume;
    [SerializeField] public float SfxVolume;
    [SerializeField] public float VoiceVolume;
    [SerializeField] public float AmbientVolume;
    
    [SerializeField] public float MouseSensitivityX;
    [SerializeField] public float MouseSensitivityY;
    [SerializeField] public float GamePadSensitivityX;
    [SerializeField] public float GamePadSensitivityY;
    
    [Button("Save Settings")]
    public void SaveSettings()
    {
        var filePath = Application.persistentDataPath + "/settings.json";
        var json = JsonUtility.ToJson(this, true);
        System.IO.File.WriteAllText(filePath, json);

        Debug.Log("Settings saved to " + filePath);
    }
    
    [Button("Load Settings")]
    public void LoadSettings()
    {
        var filePath = Application.persistentDataPath + "/settings.json";

        if (System.IO.File.Exists(filePath))
        {
            var json = System.IO.File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
        else
        {
            Debug.Log("No settings file found, loading defaults");
            LoadDefaultSettings();
        }
    }

    [Button("Load Default Settings")]
    private void LoadDefaultSettings()
    {
        this.MasterVolume = 1;
        this.MusicVolume = 1;
        this.SfxVolume = 1;
        this.VoiceVolume = 1;
        this.AmbientVolume = 1;
    }

    public void SetMasterVolume(float volume)
    {
        this.MasterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        this.MusicVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        this.SfxVolume = volume;
    }

    public void SetVoiceVolume(float volume)
    {
        this.VoiceVolume = volume;
    }

    public void SetAmbientVolume(float volume)
    {
        this.AmbientVolume = volume;
    }

    public float GetMasterVolume()
    {
        return this.MasterVolume;
    }

    public float GetMusicVolume()
    {
        return this.MusicVolume;
    }

    public float GetSfxVolume()
    {
        return this.SfxVolume;
    }

    public float GetVoiceVolume()
    {
        return this.VoiceVolume;
    }

    public float GetAmbientVolume()
    {
        return this.AmbientVolume;
    }
    
    public void SetMouseSensitivityX(float sensitivity)
    {
        this.MouseSensitivityX = sensitivity;
    }
    
    public void SetMouseSensitivityY(float sensitivity)
    {
        this.MouseSensitivityY = sensitivity;
    }
    
    public void SetGamePadSensitivityX(float sensitivity)
    {
        this.GamePadSensitivityX = sensitivity;
    }
    
    public void SetGamePadSensitivityY(float sensitivity)
    {
        this.GamePadSensitivityY = sensitivity;
    }
    
    public float GetMouseSensitivityX()
    {
        return this.MouseSensitivityX;
    }
    
    public float GetMouseSensitivityY()
    {
        return this.MouseSensitivityY;
    }
    
    public float GetGamePadSensitivityX()
    {
        return this.GamePadSensitivityX;
    }
    
    public float GetGamePadSensitivityY()
    {
        return this.GamePadSensitivityY;
    }
}