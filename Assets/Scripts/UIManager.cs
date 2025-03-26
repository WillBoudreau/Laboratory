using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    [Header("Class References")]
    [SerializeField] private GameManager gameManager;//The game manager
    [SerializeField] private LevelManager levelManager;//The level manager
    [SerializeField] private MusicHandler musicHandler;//The music handler
    [SerializeField] private LoadingScreenBehavior loadingScreenBehavior;//The loading screen behavior
    [Header("UI Elements")]
    public GameObject winMenu;//The win menu
    public GameObject mainMenu;//The main menu
    [SerializeField] private GameObject settingsMenu;//The settings menu
    public GameObject hUD;//The game menu
    [SerializeField] private GameObject pauseMenu;//The pause menu
    [SerializeField] private GameObject controlsMenu;//The controls menu
    public GameObject loadingScreen;//The loading screen
    public GameObject deathScreen;//The death screen
    [Header("Options UI Elements")]
    public Slider masterVolSlider;//The master volume slider
    public Slider musicVolSlider;//The music volume slider
    public Slider sFXVolSlider;//The sfx volume slider
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
    public CanvasGroup deathCanvasGroup;
    public float deathFadeTime;
    public List<Sprite> hudUINormal;
    public List<Sprite> hudUIDamaged;
    public GameObject hudTop;
    public GameObject hudBottom;
    public float damageFluxTime;
    public int damagePulses;
    private float vignetteTimer;
    private int pulseCounter;
    public bool vignetteActive;
    public Volume volume;
    public VolumeProfile[] profiles;

    [Header("Control Graphic Elements")]
    public GameObject tutorialLevelControllerGraphics;
    public GameObject tutorialLevelKeyboardGraphics;
    public GameObject controlsMenuControllerGraphics;
    public GameObject controlsMenuKeyboardGraphics;
    public GameObject interactPromptControllerGraphics;
    public GameObject interactPromptKeyboardGraphics;
    private bool wasGamepadActive;
    [Header("Target Buttons")]
    public GameObject menuFirstButton;
    public GameObject pauseFirstButton;
    public GameObject controlsFirstButton;
    public GameObject optionsFirstButton;
    public GameObject nextLevelButton;
    void Start()
    {
        SetUIFalse();
        LoadUI("MainMenuScene");
        ChangeControlGraphics(true);
    }
    void Update()
    {
        CheckWinCondition();
        UpdateHUD();
        ChangeControlGraphics();
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
                if(gameManager.playerCon.isGamepadActive)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(nextLevelButton);
                }
                break;
            case "MainMenuScene":
                mainMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(menuFirstButton);
                break;
            case "Settings":
                InitializeResDropDown();
                settingsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(optionsFirstButton);
                break;
            case "Game":
                hUD.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                break;
            case "Pause":
                pauseMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(pauseFirstButton);
                break;
            case "Controls":
                controlsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(controlsFirstButton);
                break;
        }
    }
    /// <summary>
    /// Update the win screen text to reflect the level completed
    /// </summary>
    void UpdateWinScreen()
    {
        int totalScenes = SceneManager.sceneCountInBuildSettings - 1;
        if(SceneManager.GetActiveScene().buildIndex == totalScenes)
        {
            winText.text = $"TEST {SceneManager.GetActiveScene().buildIndex}/{totalScenes}\nCOMPLETE\nSIMULATION COMPLETE";
        }
        else
        {
            winText.text = $"TEST {SceneManager.GetActiveScene().buildIndex}/{totalScenes}\nCOMPLETE";

        }
    }

    // <summary>
    /// Starts UI loading screen process.
    /// </summary>
    /// <param name="targetPanel"></param>
    public void UILoadingScreen(GameObject targetPanel)
    {
        StartCoroutine(LoadingUIFadeIN());
        StartCoroutine(DelayedSwitchUIPanel(fadeTime, targetPanel));
        loadingScreenBehavior.SetLoadingScreen();
    }

    // <summary>
    /// Checks and changes the control graphics around the game based on whether or not the player is using a keyboard or a controller.
    /// </summary>
    /// <returns></returns>
    public void ChangeControlGraphics(bool reset = false)
    {
        if (SceneManager.GetActiveScene().name == "L_Tutorial" && tutorialLevelControllerGraphics == null)
        {

            tutorialLevelControllerGraphics = GameObject.FindWithTag("ControllerGr");
            tutorialLevelKeyboardGraphics = GameObject.FindWithTag("KeyboardGr");
            reset = true;
            Debug.Log(GameObject.FindWithTag("ControllerGr").name);
        }
        else if (SceneManager.GetActiveScene().name != "L_Tutorial")
        {
            tutorialLevelControllerGraphics = null;
            tutorialLevelKeyboardGraphics = null;
        }

        if (wasGamepadActive == gameManager.playerCon.isGamepadActive && !reset)
        {
            return;
        }

        

        if (gameManager.playerCon.isGamepadActive)
        {

            if (tutorialLevelControllerGraphics)
            {
                tutorialLevelControllerGraphics.SetActive(true);
                tutorialLevelKeyboardGraphics.SetActive(false);
            }

            controlsMenuControllerGraphics.SetActive(true);
            interactPromptControllerGraphics.SetActive(true);

            controlsMenuKeyboardGraphics.SetActive(false);
            interactPromptKeyboardGraphics.SetActive(false);

            wasGamepadActive = true;
        }
        else
        {
            if (tutorialLevelControllerGraphics)
            {
                tutorialLevelKeyboardGraphics.SetActive(true);
                tutorialLevelControllerGraphics.SetActive(false);
            }

            controlsMenuKeyboardGraphics.SetActive(true);
            interactPromptKeyboardGraphics.SetActive(true);

            controlsMenuControllerGraphics.SetActive(false);
            interactPromptControllerGraphics.SetActive(false);

            wasGamepadActive = false;
        }
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
        SetUIFalse();
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
        float timer = 0;
        loadingBar.fillAmount = 0;
        //Debug.Log("Starting Progress Bar");
        while (timer < 1)
        {
            timer += Time.deltaTime/2f;
            loadingBar.fillAmount += Time.deltaTime/2f;
            yield return null;
        }
        yield return new WaitForEndOfFrame();
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
        if(uiPanel = mainMenu)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(menuFirstButton);
        }
    }

    private void UpdateHUD()
    {
        if(hUD.activeSelf)
        {
            if(gameManager.playerCon.isHurt)
            {
                hudTop.GetComponent<Image>().sprite = hudUIDamaged[0];
                hudBottom.GetComponent<Image>().sprite = hudUIDamaged[1];
            }
            else
            {
                hudTop.GetComponent<Image>().sprite = hudUINormal[0];
                hudBottom.GetComponent<Image>().sprite = hudUINormal[1];
            }
        }
    }

    public void playerDamage()
    {
        if(!vignetteActive)
        {
           StartCoroutine(DamageVignette());
        }
    }

    public IEnumerator DamageVignette()
    {
        vignetteActive = true;
        vignetteTimer = damageFluxTime;
        pulseCounter = damagePulses;
        
        for(pulseCounter = damagePulses; pulseCounter > 0; pulseCounter -= 1)
        {
            for(vignetteTimer = damageFluxTime;vignetteTimer > 0;)
            {
                vignetteTimer -= Time.deltaTime;
                if(pulseCounter % 2 == 0)
                {
                    volume.profile = profiles[2];
                }
                else
                {
                    volume.profile = profiles[1];
                }
                yield return null;
            }
        }
        volume.profile = profiles[0];
        vignetteActive = false;
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
