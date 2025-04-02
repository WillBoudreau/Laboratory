using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoiceLineTrigger : MonoBehaviour
{
    public int voiceLineIndex;
    public VoiceLineManager vLManager; // VL -- Voice line
    private AudioSource voiceSource;
    private bool hasBeenTriggered;
    [SerializeField] private DisplayDialogue displayDialogue;
    void Awake()
    {
        vLManager = FindObjectOfType<VoiceLineManager>();
        voiceSource = vLManager.vLSource;
        hasBeenTriggered = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            displayDialogue.SetDialogue();
            vLManager.PlayVoiceLine(vLManager.voiceLines[voiceLineIndex]);
        }
    }
}
