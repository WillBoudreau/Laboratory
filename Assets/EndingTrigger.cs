using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;//The player object
    [SerializeField] private DisplayDialogue displayDialogue;//The display dialogue object
    private Coroutine endingRoutine;
    void Start()
    {
        //If the player is not set, find the player
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //If the object is the player
        if(other.gameObject == player)
        {
            player.GetComponent<PlayerController>().StartEnding();
            displayDialogue.SetDialogue(displayDialogue.dialogueTextDisplayPanel);
        }
    }
}
