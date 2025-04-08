using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserVLTrigger : MonoBehaviour
{
    private VoiceLineManager voiceLineManager;
    public PlayerController player;
    private bool hasBeenTriggered;
    [SerializeField] private DisplayDialogue displayDialogue;
    [SerializeField] private bool displayedDialogue;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        hasBeenTriggered = false;
        voiceLineManager = FindObjectOfType<VoiceLineManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = FindObjectOfType<PlayerController>();
            player.inDangerZone = true;
        }   
    }
    private void OnTriggerStay(Collider other)
    {
        if (player.hasITYSTrigger == true && !displayedDialogue)
        {
            Debug.Log("Displaying dialogue");
            displayDialogue.SetDialogue(displayDialogue.dialogueTextDisplayPanel);
            displayedDialogue = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.inDangerZone = false;
            //player = null;
        }
    }
}
