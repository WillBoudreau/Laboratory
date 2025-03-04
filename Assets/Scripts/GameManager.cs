using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Object References")]
    public GameObject player;
    public UIManager uiManager;
    public LevelManager levelManager;
    public PlayerController playerCon;
    public enum GameState{MainMenu, Gameplay, Paused, GameEnd}
    public GameState gameState;
    public GameState prevState;
    [Header("Game Settings")]
    public bool isPaused;//Is the game paused
    public bool hasWon;//Has the player won
    

    void Start()
    {
        gameState = GameState.MainMenu;
    }
    private void Update()
    {
        CheckGamePause();
    }
    void CheckGamePause()
    {
        //If the game is paused, time scale is 0
        if (isPaused | hasWon)
        {
            Time.timeScale = 0;
        }
        //If the game is not paused, time scale is 1
        else if (!isPaused)
        {
            Time.timeScale = 1;
        }
    }
    public void RestartGame()
    {
        hasWon = false;
        isPaused = false;
    }
    public void NextLevel()
    {
        hasWon = false;
        isPaused = false;
    }
    // public void WinGame()
    // {
    //     hasWon = true;
    // }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        playerCon.inputEnabled = false;
        uiManager.LoadUI("Pause");
    }
    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        playerCon.inputEnabled = true;
        uiManager.LoadUI("Game");
    }

    public void ChangeGameState(GameState newState)
    {
        prevState = gameState;
        gameState = newState;
    }
    /// <summary>
    /// Debug controls to load level 1.
    /// </summary>
    void OnDebugLoadLevel1()
    {
        if(playerCon.debugMode)
        {
            levelManager.DebugLoadScene("L_1");
        }
    }

    /// <summary>
    /// Debug controls to load level 2.
    /// </summary>
    void OnDebugLoadLevel2()
    {
        if(playerCon.debugMode)
        {
            levelManager.DebugLoadScene("L_2");
        }
    }

    /// <summary>
    /// Debug controls to load level 3.
    /// </summary>
    void OnDebugLoadLevel3()
    {
        if(playerCon.debugMode)
        {
            levelManager.DebugLoadScene("L_3");   
        }
    }
}
