using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private bool isPaused;//Is the game paused
    public bool hasWon;//Has the player won

    private void Update()
    {
        //If the game is paused, time scale is 0
        if(isPaused | hasWon)
        {
            Time.timeScale = 0;
        }
        //If the game is not paused, time scale is 1
        else
        {
            Time.timeScale = 1;
        }
    }
    public void RestartGame()
    {
        hasWon = false;
    }
    public void WinGame()
    {
        hasWon = true;
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
    }
    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
    }
}