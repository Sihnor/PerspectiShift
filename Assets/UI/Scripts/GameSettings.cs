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
        MasterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        this.SfxVolume = volume;
    }

    public void SetVoiceVolume(float volume)
    {
        VoiceVolume = volume;
    }

    public void SetAmbientVolume(float volume)
    {
        AmbientVolume = volume;
    }

    public float GetMasterVolume()
    {
        return MasterVolume;
    }

    public float GetMusicVolume()
    {
        return MusicVolume;
    }

    public float GetSfxVolume()
    {
        return this.SfxVolume;
    }

    public float GetVoiceVolume()
    {
        return VoiceVolume;
    }

    public float GetAmbientVolume()
    {
        return AmbientVolume;
    }
}