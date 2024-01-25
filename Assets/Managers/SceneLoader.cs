using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    _SampleScene = 8,
    None = 9
}

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private ESceneIndices StartScene = ESceneIndices.MainMenu;

    [SerializeField] private GameObject LoadingScreenObject;
    [SerializeField] private Image LoadingScreenImage;
    [SerializeField] private AnimationCurve LoadingScreenCurve;
    [SerializeField] private float MinimumLoadingTime = 10;

    private List<ESceneIndices> AdditiveScenes;
    private List<ESceneIndices> NonAdditiveScenes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.LoadingScreenObject);
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        this.NonAdditiveScenes = new List<ESceneIndices>();
        this.AdditiveScenes = new List<ESceneIndices>();

        LoadScene(this.StartScene);
    }



    // Load the scene.
    public void LoadScene(ESceneIndices sceneIndex)
    {
        this.NonAdditiveScenes.Add(this.StartScene);
        SceneManager.LoadScene(sceneIndex.ToString(), LoadSceneMode.Single);
    }

    // Load the scene asynchronously.
    public void LoadSceneAsync(ESceneIndices sceneIndex, bool loadingScreen = false)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)sceneIndex);

        if (!loadingScreen)
        {
            this.NonAdditiveScenes.Add(this.StartScene);
            return;
        }

        StartCoroutine(ProgressRoutine(asyncLoad));
    }

    // Load the scene additively.
    public void LoadSceneAdditive(ESceneIndices sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex.ToString(), LoadSceneMode.Additive);
        this.AdditiveScenes.Add(sceneIndex);
    }

    // Load the scene additively asynchronously.
    private void LoadSceneAdditiveAsync(ESceneIndices sceneIndex)
    {
        SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive);
        this.AdditiveScenes.Add(sceneIndex);
    }

    // Unload the scene.
    public void UnloadScene(ESceneIndices sceneIndex)
    {
        StartCoroutine(UnloadSceneAsync(sceneIndex));
    }

    // Unload the scene asynchronously.
    private IEnumerator UnloadSceneAsync(ESceneIndices sceneIndex)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync((int)sceneIndex);
        this.NonAdditiveScenes.Remove(sceneIndex);

        while (!asyncUnload.isDone)
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
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        this.NonAdditiveScenes.Clear();
        this.AdditiveScenes.Clear();
        
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
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene(),
            UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        this.NonAdditiveScenes.Clear();
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator ProgressRoutine(AsyncOperation asyncOperation)
    {
        this.LoadingScreenObject.SetActive(true);
        this.LoadingScreenImage.fillAmount = 0;
        asyncOperation.allowSceneActivation = false;

        float counter = 0;

        while (!Mathf.Approximately(asyncOperation.progress, 0.9f) || counter < this.MinimumLoadingTime)
        {
            yield return new WaitForEndOfFrame();
            counter += Time.deltaTime;
            float progress = Mathf.Min(asyncOperation.progress /0.9f, this.LoadingScreenCurve.Evaluate(counter/ this.MinimumLoadingTime));
            this.LoadingScreenImage.fillAmount = progress;
        }

        asyncOperation.allowSceneActivation = true;
        this.LoadingScreenImage.fillAmount = 1;
        this.LoadingScreenObject.SetActive(false);
    }
}