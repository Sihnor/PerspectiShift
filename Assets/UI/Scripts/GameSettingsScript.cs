using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsScript : MonoBehaviour
{
    [SerializeField] private Slider MouseSensitivityXSlider;
    [SerializeField] private Slider MouseSensitivityYSlider;
    [SerializeField] private Slider GamePadSensitivityXSlider;
    [SerializeField] private Slider GamePadSensitivityYSlider;
    
    [SerializeField] private TextMeshProUGUI MouseSensitivityXText;
    [SerializeField] private TextMeshProUGUI MouseSensitivityYText;
    [SerializeField] private TextMeshProUGUI GamePadSensitivityXText;
    [SerializeField] private TextMeshProUGUI GamePadSensitivityYText;
    
    [SerializeField] private GameSettings GameSettings;
    
    // Start is called before the first frame update
    void Start()
    {
        this.MouseSensitivityXSlider.onValueChanged.AddListener(SetMouseSensitivityX);
        this.MouseSensitivityYSlider.onValueChanged.AddListener(SetMouseSensitivityY);
        this.GamePadSensitivityXSlider.onValueChanged.AddListener(SetGamePadSensitivityX);
        this.GamePadSensitivityYSlider.onValueChanged.AddListener(SetGamePadSensitivityY);
        
        this.MouseSensitivityXSlider.value = this.GameSettings.GetMouseSensitivityX();
        this.MouseSensitivityYSlider.value = this.GameSettings.GetMouseSensitivityY();
        this.GamePadSensitivityXSlider.value = this.GameSettings.GetGamePadSensitivityX();
        this.GamePadSensitivityYSlider.value = this.GameSettings.GetGamePadSensitivityY();
        
        this.MouseSensitivityXText.text = this.MouseSensitivityXSlider.value.ToString("F2");
        this.MouseSensitivityYText.text = this.MouseSensitivityYSlider.value.ToString("F2");
        this.GamePadSensitivityXText.text = this.GamePadSensitivityXSlider.value.ToString("F2");
        this.GamePadSensitivityYText.text = this.GamePadSensitivityYSlider.value.ToString("F2");
    }

    private void SetMouseSensitivityX(float sensitivity)
    {
        this.GameSettings.SetMouseSensitivityX(sensitivity);
        this.MouseSensitivityXText.text = sensitivity.ToString("F2");
    }
    
    private void SetMouseSensitivityY(float sensitivity)
    {
        this.GameSettings.SetMouseSensitivityY(sensitivity);
        this.MouseSensitivityYText.text = sensitivity.ToString("F2");
    }
    
    private void SetGamePadSensitivityX(float sensitivity)
    {
        this.GameSettings.SetGamePadSensitivityX(sensitivity);
        this.GamePadSensitivityXText.text = sensitivity.ToString("F2");
    }
    
    private void SetGamePadSensitivityY(float sensitivity)
    {
        this.GameSettings.SetGamePadSensitivityY(sensitivity);
        this.GamePadSensitivityYText.text = sensitivity.ToString("F2");
    }
   
}
