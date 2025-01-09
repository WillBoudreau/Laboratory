using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private UIManager uiManager;//The UI Manager
    void Start()
    {
        //If the UIManager is null, find the UIManager
        if(uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
    }
    /// <summary>
    /// Load the specified level based on the sceneName variable
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        uiManager.LoadUI(sceneName);
    }
    /// <summary>
    /// Quit the game function
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
