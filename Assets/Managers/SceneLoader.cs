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

    // Load the scene.
    public void LoadScene(ESceneIndices sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    
    // Load the scene asynchronously.
    private IEnumerator LoadSceneAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    // Load the scene additively.
    public void LoadSceneAdditive(ESceneIndices sceneIndex)
    {
        StartCoroutine(LoadSceneAdditiveAsync(sceneIndex));
    }
    
    // Load the scene additively asynchronously.
    private IEnumerator LoadSceneAdditiveAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)sceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    // Unload the scene.
    public void UnloadScene(ESceneIndices sceneIndex)
    {
        StartCoroutine(UnloadSceneAsync(sceneIndex));
    }
    
    // Unload the scene asynchronously.
    private IEnumerator UnloadSceneAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    // Unload the scene additively.
    public void UnloadSceneAdditive(ESceneIndices sceneIndex)
    {
        StartCoroutine(UnloadSceneAdditiveAsync(sceneIndex));
    }
    
    // Unload the scene additively asynchronously.
    private IEnumerator UnloadSceneAdditiveAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)sceneIndex, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    // Unload all scenes.
    public void UnloadAllScenes()
    {
        StartCoroutine(UnloadAllScenesAsync());
    }
    
    // Unload all scenes asynchronously.
    private IEnumerator UnloadAllScenesAsync()
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    // Unload all scenes additively.
    public void UnloadAllScenesAdditive()
    {
        StartCoroutine(UnloadAllScenesAdditiveAsync());
    }
    
    // Unload all scenes additively asynchronously.
    private IEnumerator UnloadAllScenesAdditiveAsync()
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    // Unload all scenes except.
    public void UnloadAllScenesExcept(ESceneIndices sceneIndex)
    {
        StartCoroutine(UnloadAllScenesExceptAsync(sceneIndex));
    }
    
    // Unload all scenes except asynchronously.
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
    
    // Unload all scenes except additively.
    public void UnloadAllScenesExceptAdditive(ESceneIndices sceneIndex)
    {
        StartCoroutine(UnloadAllScenesExceptAdditiveAsync(sceneIndex));
    }
    
    // Unload all scenes except additively asynchronously.
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
