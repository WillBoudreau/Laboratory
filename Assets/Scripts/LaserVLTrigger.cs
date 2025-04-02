using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserVLTrigger : MonoBehaviour
{
    private VoiceLineManager voiceLineManager;
    public PlayerController player;
    private bool hasBeenTriggered;

    void Awake()
    {
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


    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.inDangerZone = false;
            player = null;
        }
    }
}
