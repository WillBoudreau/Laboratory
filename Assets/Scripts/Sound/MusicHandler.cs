using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicHandler : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private string startingTrack;
    [SerializeField] private float titleVolume;
    [SerializeField] private float levelVolume;
    [SerializeField] private AudioSource musicPlayer;
    public AudioMixer mixer;

    [Header("Sound References")]
    [SerializeField] private AudioClip titleMusic;
    [SerializeField] private AudioClip levelMusic;
    private string currentTrackName;

    // Start is called before the first frame update
    void Start()
    {
        currentTrackName = "";
        SwitchAudioTrack("title");
    }

    public void SwitchAudioTrack(string name)
    {
        if (name == currentTrackName)
            return;

        if (musicPlayer.isPlaying)
            musicPlayer.Stop();

        switch (name)
        {
            case "title":
                musicPlayer.clip = titleMusic;
                break;

            case "level":
                musicPlayer.clip = levelMusic;
                break;

            default:
                break;
        }
        currentTrackName = name;
        musicPlayer.Play();
    }


    public void ChangeVolume(string group, float value)
    {
        mixer.SetFloat(group,value);
    }

    void Update()
    {
        
    }
}
