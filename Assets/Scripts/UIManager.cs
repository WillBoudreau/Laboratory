using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Class References")]
    [SerializeField] private GameManager gameManager;//The game manager
    [SerializeField] private LevelManager levelManager;
    [Header("UI Elements")]
    [SerializeField] private GameObject winMenu;//The win menu
    [SerializeField] private GameObject mainMenu;//The main menu
    [SerializeField] private GameObject settingsMenu;//The settings menu
    [SerializeField] private GameObject gameMenu;//The game menu
    [SerializeField] private GameObject pauseMenu;//The pause menu
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI winText;//The win text
    [Header("Loading Screen UI Elements")]
    public GameObject loadingScreen;
    public CanvasGroup loadingScreenCanvasGroup;
    public Image loadingBar;
    public float fadeTime;
    void Start()
    {
        SetUIFalse();
        LoadUI("MainMenuScene");
    }
    void Update()
    {
        CheckWinCondition();
    }
    /// <summary>
    /// Check the win condition
    /// </summary>
    void CheckWinCondition()
    {
        //If the player has won, load the win UI
        if(gameManager.hasWon)
        {
            LoadUI("Win");
        }
    }
    /// <summary>
    /// Set the UI elements to false
    /// </summary>
    void SetUIFalse()
    {
        winMenu.SetActive(false);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }
    /// <summary>
    /// Load the UI element based on the ui string
    /// </summary>
    /// <param name="ui">ui is used to load the appropriate ui</param>
    public void LoadUI(string ui)
    {
        SetUIFalse();
        //Load the UI based on the string
        switch (ui)
        {
            case "Win":
                UpdateWinScreen();
                gameManager.WinGame();
                winMenu.SetActive(true);
                break;
            case "MainMenuScene":
                mainMenu.SetActive(true);
                break;
            case "Settings":
                settingsMenu.SetActive(true);
                break;
            case "Game":
                gameManager.ResumeGame();
                gameMenu.SetActive(true);
                break;
            case "Pause":
                gameManager.PauseGame();
                pauseMenu.SetActive(true);
                break;
        }
    }
    /// <summary>
    /// Update the win screen text to reflect the level completed
    /// </summary>
    void UpdateWinScreen()
    {
        winText.text = $"TEST {SceneManager.GetActiveScene().buildIndex}/3\nCOMPLETE";
    }

    // <summary>
    /// Starts UI loading screen process.
    /// </summary>
    /// <param name="targetPanel"></param>
    public void UILoadingScreen(GameObject targetPanel)
    {
        StartCoroutine(LoadingUIFadeIN());
        StartCoroutine(DelayedSwitchUIPanel(fadeTime, targetPanel));
    }

    /// <summary>
    /// Fades loading scnreen out.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadingUIFadeOut()
    {
        //Debug.Log("Starting Fade out");

        float timer = 0;

        while (timer < fadeTime)
        {
            loadingScreenCanvasGroup.alpha = Mathf.Lerp(1, 0, timer/fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        loadingScreenCanvasGroup.alpha = 0;
        loadingScreen.SetActive(false);
        loadingBar.fillAmount = 0;
        //Debug.Log("Ending Fade out");
    }
    /// <summary>
    /// Fades Loading screen in.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadingUIFadeIN()
    {
        //Debug.Log("Starting Fade in");
        float timer = 0;
        loadingScreen.SetActive(true);

        while (timer < fadeTime)
        {
            loadingScreenCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        loadingScreenCanvasGroup.alpha = 1;

        //Debug.Log("Ending Fadein");
        StartCoroutine(LoadingBarProgress());
    }
    /// <summary>
    /// Sets the loading bar progress to average progress of all loading. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadingBarProgress()
    {
        //Debug.Log("Starting Progress Bar");
        while (levelManager.scenesToLoad.Count <= 0)
        {
            //waiting for loading to begin
            yield return null;
        }
        while (levelManager.scenesToLoad.Count > 0)
        {
            loadingBar.fillAmount = levelManager.GetLoadingProgress();
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        //Debug.Log("Ending Progress Bar");
        StartCoroutine(LoadingUIFadeOut());
    }
    /// <summary>
    /// used for fade in fade out for loading screen UI. 
    /// </summary>
    /// <param name="time"></param>
    /// <param name="uiPanel"></param>
    /// <returns></returns>
    private IEnumerator DelayedSwitchUIPanel(float time, GameObject uiPanel)
    {
        yield return new WaitForSeconds(time);
        SetUIFalse();
        uiPanel.SetActive(true);
    }
}
