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
    [SerializeField] private MusicHandler musicHandler;
    [Header("UI Elements")]
    public GameObject winMenu;//The win menu
    public GameObject mainMenu;//The main menu
    [SerializeField] private GameObject settingsMenu;//The settings menu
    public GameObject hUD;//The game menu
    [SerializeField] private GameObject pauseMenu;//The pause menu
    [SerializeField] private GameObject controlsMenu;//The controls menu
    public GameObject loadingScreen;
    public GameObject deathScreen;
    [Header("Options UI Elements")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sFXVolSlider;
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    public Toggle fullscreenToggle;
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI winText;//The win text
    [Header("Loading Screen UI Elements")]
    public CanvasGroup loadingScreenCanvasGroup;
    public Image loadingBar;
    public float fadeTime;
    [Header("HUD UI Elements")]
    public GameObject hurtIndicator;
    public CanvasGroup deathCanvasGroup;
    public float deathFadeTime;

    void Start()
    {
        SetUIFalse();
        LoadUI("MainMenuScene");
    }
    void Update()
    {
        CheckWinCondition();
        UpdateHUD();
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
        hUD.SetActive(false);
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(false);
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
                winMenu.SetActive(true);
                break;
            case "MainMenuScene":
                mainMenu.SetActive(true);
                break;
            case "Settings":
                InitializeResDropDown();
                settingsMenu.SetActive(true);
                break;
            case "Game":
                hUD.SetActive(true);
                break;
            case "Pause":
                pauseMenu.SetActive(true);
                break;
            case "Controls":
                controlsMenu.SetActive(true);
                break;
        }
    }
    /// <summary>
    /// Update the win screen text to reflect the level completed
    /// </summary>
    void UpdateWinScreen()
    {
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        winText.text = $"TEST {SceneManager.GetActiveScene().buildIndex}/{totalScenes}\nCOMPLETE";
    }

    // <summary>
    /// Starts UI loading screen process.
    /// </summary>
    /// <param name="targetPanel"></param>
    public void UILoadingScreen(GameObject targetPanel)
    {
        StartCoroutine(LoadingUIFadeIN());
        StartCoroutine(DelayedSwitchUIPanel(levelManager.minLoadTime, targetPanel));
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

    private void UpdateHUD()
    {
        if(hUD.activeSelf)
        {
            hurtIndicator.SetActive(gameManager.playerCon.isHurt);
        }
    }

    /// <summary>
    /// Fades Death screen out.
    /// </summary>
    /// <returns></returns>
    public IEnumerator DeathUIFadeOut()
    {
        //Debug.Log("Starting Fade out");

        float timer = 0;

        while (timer < fadeTime)
        {
            deathCanvasGroup.alpha = Mathf.Lerp(1, 0, timer/deathFadeTime);
            timer += Time.deltaTime;
            yield return null;
        }
        deathCanvasGroup.alpha = 0;
        deathScreen.SetActive(false);
        //Debug.Log("Ending Fade out");
    }
    /// <summary>
    /// Fades death screen in.
    /// </summary>
    /// <returns></returns>
    public IEnumerator DeathUIFadeIN()
    {
        deathScreen.SetActive(true);
        //Debug.Log("Starting Fade in");
        float timer = 0;
        loadingScreen.SetActive(true);
        while (timer < fadeTime)
        {
            deathCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / deathFadeTime);
            timer += Time.deltaTime;
            yield return null;
        }
        deathCanvasGroup.alpha = 1;
    }
    /// <summary>
    /// Sets sliders value to base volume level
    /// </summary>
    public void GetStartingVolume()
    {
        if(musicHandler.mixer.GetFloat("MasterVol",out float masterValue))
        {
            masterVolSlider.value = masterValue;
        }
        if(musicHandler.mixer.GetFloat("MusicVol",out float musicValue))
        {
            musicVolSlider.value = musicValue;
        }
        if(musicHandler.mixer.GetFloat("SFXVol", out float sfxValue))
        {
            sFXVolSlider.value = sfxValue;
        }
    }
    /// <summary>
    /// Used by slider to pass value to sound manager
    /// </summary>
    /// <param name="group"></param>
    public void SliderVolume(string group)
    {
        switch(group)
        {
            case "MasterVol":
                musicHandler.ChangeVolume(group,masterVolSlider.value);
                break;
            case "MusicVol":
                musicHandler.ChangeVolume(group,musicVolSlider.value);
                break;
            case "SFXVol":
                musicHandler.ChangeVolume(group,sFXVolSlider.value);
                break;
        }
    }

    /// <summary>
    /// Used by setting menu to go back to correct screen when accessed from paused menu or main menu.
    /// </summary>
    public void BackFromOptions()
    {
        if(gameManager.gameState == GameManager.GameState.MainMenu)
        {
            LoadUI("MainMenuScene");
        }
        else if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            LoadUI("Pause");
        }
    }

    /// <summary>
    /// Sets all dropdown options to available resolutions on device. 
    /// </summary>
    private void InitializeResDropDown()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> Options = new List<string>();

        int CurrentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string Option = string.Format("{0} X {1}", resolutions[i].width, resolutions[i].height);
            Options.Add(Option);

            if(resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(Options);
        resolutionDropdown.value = CurrentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    /// <summary>
    /// Sets Resolution, used by dropdown object on value change.
    /// </summary>
    public void SetResolution()
    {
        Resolution resolution = resolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    /// <summary>
    /// fullscreen toggle
    /// </summary>
    public void SetFullScreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
    }
}
