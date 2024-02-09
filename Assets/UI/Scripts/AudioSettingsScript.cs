using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsScript : MonoBehaviour
{
    [SerializeField] private Slider MasterVolumeSlider;
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Slider SfxVolumeSlider;
    [SerializeField] private Slider VoiceVolumeSlider;
    [SerializeField] private Slider AmbientVolumeSlider;

    [SerializeField] private GameSettings GameSettings;
    
    // Start is called before the first frame update
    void Start()
    {
        this.MasterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        this.MusicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        this.SfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        this.VoiceVolumeSlider.onValueChanged.AddListener(SetVoiceVolume);
        this.AmbientVolumeSlider.onValueChanged.AddListener(SetAmbientVolume);
        
        this.MasterVolumeSlider.value = this.GameSettings.GetMasterVolume();
        this.MusicVolumeSlider.value = this.GameSettings.GetMusicVolume();
        this.SfxVolumeSlider.value = this.GameSettings.GetSfxVolume();
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
    
    private void SetSfxVolume(float volume)
    {
        this.GameSettings.SetSfxVolume(volume);
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
