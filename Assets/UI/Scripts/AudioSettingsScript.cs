using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsScript : MonoBehaviour
{
    [SerializeField] private Slider MasterVolumeSlider;
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider VoiceVolumeSlider;
    [SerializeField] private Slider AmbientVolumeSlider;

    [SerializeField] private GameSettings GameSettings;
    
    // Start is called before the first frame update
    void Start()
    {
        this.MasterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        this.MusicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        this.SFXVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        this.VoiceVolumeSlider.onValueChanged.AddListener(SetVoiceVolume);
        this.AmbientVolumeSlider.onValueChanged.AddListener(SetAmbientVolume);
        
        this.MasterVolumeSlider.value = this.GameSettings.GetMasterVolume();
        this.MusicVolumeSlider.value = this.GameSettings.GetMusicVolume();
        this.SFXVolumeSlider.value = this.GameSettings.GetSFXVolume();
        this.VoiceVolumeSlider.value = this.GameSettings.GetVoiceVolume();
        this.AmbientVolumeSlider.value = this.GameSettings.GetAmbientVolume();
    }

    private void SetMasterVolume(float volume)
    {
        this.GameSettings.SetMasterVolume(volume);
    }
    
    private void SetMusicVolume(float volume)
    {
        this.GameSettings.SetMusicVolume(volume);
    }
    
    private void SetSFXVolume(float volume)
    {
        this.GameSettings.SetSFXVolume(volume);
    }
    
    private void SetVoiceVolume(float volume)
    {
        this.GameSettings.SetVoiceVolume(volume);
    }
    
    private void SetAmbientVolume(float volume)
    {
        this.GameSettings.SetAmbientVolume(volume);
    }
    
    
}
