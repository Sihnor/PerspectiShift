using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    public void OpenMainMenu()
    {
        SceneLoader.Instance.LoadScene(ESceneIndices.MainMenu);
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
