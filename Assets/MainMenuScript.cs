using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button StartButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button QuitButton;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(StartGame);
        SettingsButton.onClick.AddListener(OpenSettings);
        QuitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        SceneLoader.Instance.LoadScene(ESceneIndices.Game);
    }

    private void OpenSettings()
    {
        SceneLoader.Instance.LoadScene(ESceneIndices.Settings);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }
}