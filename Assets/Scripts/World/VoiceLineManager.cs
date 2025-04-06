using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoiceLineManager : MonoBehaviour
{
    public List<AudioClip> voiceLines;
    public AudioSource vLSource; //VL = voice line
    public GameObject[] speakers;
    public GameObject firstDoor;


    
    public void PlayVoiceLine(AudioClip clip)
    {
        vLSource.clip = clip;
        vLSource.Play();
    }

    public void GetAllSpeakers()
    {
        speakers = GameObject.FindGameObjectsWithTag("Speaker");
    }

    void Update()
    {
        if(speakers[0] != null)
        {
            foreach(GameObject speaker in speakers)
            {
                speaker.GetComponent<SpeakerController>().isPlaying = vLSource.isPlaying;
            }
        }
        if(firstDoor!= null)
        {
            if(vLSource.isPlaying)
            {
                firstDoor.GetComponent<DoorBehaviour>().canOpen = false;
            }
            else
            {
                firstDoor.GetComponent<DoorBehaviour>().canOpen = true;
            }
        }
    }
}
