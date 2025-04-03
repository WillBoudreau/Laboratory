using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoiceLineTrigger : MonoBehaviour
{
    public int voiceLineIndex;
    public VoiceLineManager vLManager; // VL -- Voice line
    private AudioSource voiceSource;
    [SerializeField]private bool hasBeenTriggered;
    [SerializeField] private DisplayDialogue displayDialogue;
    void Awake()
    {
        vLManager = FindObjectOfType<VoiceLineManager>();
        voiceSource = vLManager.vLSource;
        hasBeenTriggered = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !hasBeenTriggered && vLManager != null)
        {
            hasBeenTriggered = true;
            if(displayDialogue != null)
            {
                displayDialogue.SetDialogue(displayDialogue.dialogueTextDisplayPanel);
            }
            else
            {
                Debug.Log("DisplayDialogue is null!");
            }
            vLManager.PlayVoiceLine(vLManager.voiceLines[voiceLineIndex]);
        }
        else
        {
            Debug.Log("Something is wrong with this trigger!");
        }
    }
}
