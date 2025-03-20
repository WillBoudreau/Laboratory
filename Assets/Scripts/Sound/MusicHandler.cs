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
    [SerializeField] private AudioClip tutorialMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip level2Music;
    [SerializeField] private AudioClip level3Music;
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
            case "tutorial":
                musicPlayer.clip = tutorialMusic;
                break;
            case "level1":
                musicPlayer.clip = levelMusic;
                break;
            case "level2":
                musicPlayer.clip = level2Music;
                break;
            case "level3":
                musicPlayer.clip = level3Music;
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
