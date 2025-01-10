using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;//The player object
    [SerializeField] private GameManager gameManager;//The game manager object
    void Start()
    {
        //If the player is not set, find the player
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        //If the game manager is not set, find the game manager
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //If the object is the player
        if(other.gameObject == player)
        {
            //Check for win
            gameManager.WinGame();
        }
    }

}
