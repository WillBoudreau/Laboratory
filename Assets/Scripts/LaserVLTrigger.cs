using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserVLTrigger : MonoBehaviour
{
    private VoiceLineManager voiceLineManager;
    private PlayerController player;
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
        }
    }

    void Update()
    {
        if(player != null && !hasBeenTriggered)
        {
            if(player.isDead)
            {
                voiceLineManager.PlayVoiceLine(voiceLineManager.voiceLines[13]);
                hasBeenTriggered = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = null;
        }
    }
}
