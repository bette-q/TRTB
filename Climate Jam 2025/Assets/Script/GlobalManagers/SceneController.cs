using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    // Register root objects of the main scene to toggle
    [Header("Main Scene Objects (Cameras/UI/etc)")]
    public List<GameObject> mainSceneRoots;
    public string mainScene;
    private string curScene;

    private void Awake()
    {
        // Standard singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject); // Persist across scenes

        AutoLoadMainSceneRoots();
    }
    public void AutoLoadMainSceneRoots()
    {
        mainSceneRoots.Clear();

        // Only do this for the active scene at startup
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] roots = activeScene.GetRootGameObjects();

        foreach (var go in roots)
        {
            // Optional: add your own filter here (e.g., only objects with a tag/layer)
            mainSceneRoots.Add(go);
        }
    }

    // Enter secondary (additive) scene, e.g., SphereScene
    public void EnterAdditiveScene(string sceneName)
    {
        if(sceneName.Equals(mainScene)) 
        {
            ExitAdditiveScene(curScene);
            return;
        }

        curScene = sceneName;
        SetMainSceneRootsActive(false);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    // Exit secondary scene and return to main
    public void ExitAdditiveScene(string sceneName)
    {
        curScene = mainScene;
        SceneManager.UnloadSceneAsync(sceneName);
        SetMainSceneRootsActive(true);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(mainScene);
    }

    // Utility: Show/hide all root objects
    private void SetMainSceneRootsActive(bool value)
    {
        foreach (var go in mainSceneRoots)
            if (go != null)
                go.SetActive(value);
    }

    // Optional: Add async overloads, loading screen triggers, etc.
}
