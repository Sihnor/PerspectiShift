using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESceneIndices
{
    InitScene = 0,
    MainMenu = 1,
    Game = 2,
    GameOver = 3,
    Credits = 4,
    Settings = 5,
    Loading = 6,
    Pause = 7,
    None = 8
}

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    
    [SerializeField]
    private ESceneIndices StartScene = ESceneIndices.MainMenu;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        LoadScene(this.StartScene);
    }

    public void LoadScene(ESceneIndices sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    
    private IEnumerator LoadSceneAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public void LoadSceneAdditive(ESceneIndices sceneIndex)
    {
        StartCoroutine(LoadSceneAdditiveAsync(sceneIndex));
    }
    
    private IEnumerator LoadSceneAdditiveAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)sceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public void UnloadScene(ESceneIndices sceneIndex)
    {
        StartCoroutine(UnloadSceneAsync(sceneIndex));
    }
    
    private IEnumerator UnloadSceneAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public void UnloadSceneAdditive(ESceneIndices sceneIndex)
    {
        StartCoroutine(UnloadSceneAdditiveAsync(sceneIndex));
    }
    
    private IEnumerator UnloadSceneAdditiveAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)sceneIndex, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public void UnloadAllScenes()
    {
        StartCoroutine(UnloadAllScenesAsync());
    }
    
    private IEnumerator UnloadAllScenesAsync()
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public void UnloadAllScenesAdditive()
    {
        StartCoroutine(UnloadAllScenesAdditiveAsync());
    }
    
    private IEnumerator UnloadAllScenesAdditiveAsync()
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public void UnloadAllScenesExcept(ESceneIndices sceneIndex)
    {
        StartCoroutine(UnloadAllScenesExceptAsync(sceneIndex));
    }
    
    private IEnumerator UnloadAllScenesExceptAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)sceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public void UnloadAllScenesExceptAdditive(ESceneIndices sceneIndex)
    {
        StartCoroutine(UnloadAllScenesExceptAdditiveAsync(sceneIndex));
    }
    
    private IEnumerator UnloadAllScenesExceptAdditiveAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)sceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
