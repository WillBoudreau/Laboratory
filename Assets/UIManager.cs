using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject winMenu;//The win menu
    [SerializeField] private GameObject mainMenu;//The main menu
    [SerializeField] private GameObject settingsMenu;//The settings menu
    [SerializeField] private GameObject gameMenu;//The game menu
    [SerializeField] private GameObject pauseMenu;//The pause menu
    [Header("Class References")]
    [SerializeField] private GameManager gameManager;//The game manager
    void Start()
    {
        SetUIFalse();
        LoadUI("MainMenuScene");
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
                winMenu.SetActive(true);
                break;
            case "MainMenuScene":
                mainMenu.SetActive(true);
                break;
            case "Settings":
                settingsMenu.SetActive(true);
                break;
            case "GameScene":
                gameManager.ResumeGame();
                gameMenu.SetActive(true);
                break;
            case "Pause":
                gameManager.PauseGame();
                pauseMenu.SetActive(true);
                break;
        }
    }
}
