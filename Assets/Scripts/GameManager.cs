using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Object References")]
    public GameObject player;
    public UIManager uiManager;
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
        uiManager.LoadUI("Pause");
    }
    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        uiManager.LoadUI("Game");
    }
}
