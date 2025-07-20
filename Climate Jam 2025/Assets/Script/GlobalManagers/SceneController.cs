using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    // Register root objects of the main scene to toggle
    [Header("Main Scene Objects (Cameras/UI/etc)")]
    public List<GameObject> mainSceneRoots;

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
    }

    // Enter secondary (additive) scene, e.g., SphereScene
    public void EnterAdditiveScene(string sceneName)
    {
        SetMainSceneRootsActive(false);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    // Exit secondary scene and return to main
    public void ExitAdditiveScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
        SetMainSceneRootsActive(true);
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
