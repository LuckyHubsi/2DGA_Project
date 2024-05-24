using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class is an example of a SceneManager
/// </summary>
/// 

public class ScenesManager : MonoBehaviour
{
    // creating a singleton instance that can be used in any script without instantiating it first
    public static ScenesManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    // creating an enum for all Scenes that we want to use in our build
    // to access the scenes that are currently in the build go to "File > Build Settings > Scenes in Build"
    // the names in the enum have to match the names of your scenes
    public enum Scene { 
        MenuScene,
        MainScene,
        GameOverScene
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LoadScene(Scene scene) {
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {

    }
}
