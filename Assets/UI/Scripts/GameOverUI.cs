using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public void RestartGame()
    {
        SceneLoader.Instance.LoadScene(ESceneIndices.MainMenu);
    }
}
