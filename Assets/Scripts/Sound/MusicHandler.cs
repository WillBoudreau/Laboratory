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

    [Header("References")]
    public Slider volumeSlider;
    [SerializeField] private AudioSource musicPlayer;
    public AudioMixer mixer;

    [Header("Sound References")]
    [SerializeField] private AudioClip titleMusic;
    [SerializeField] private AudioClip levelMusic;

    private float currentTrackVolume;
    private string currentTrackName;
    private bool isLowerVolume;

    // Start is called before the first frame update
    void Start()
    {
        isLowerVolume = false;

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

        currentTrackVolume = musicPlayer.volume;
        currentTrackName = name;

        isLowerVolume = false;

        if (volumeSlider.value != 0)
            musicPlayer.volume = musicPlayer.volume / volumeSlider.value;
        else
            musicPlayer.volume = 0;

        musicPlayer.Play();
    }

    public void LowerVolume()
    {
        isLowerVolume = true;
    }

    public void RestoreVolume()
    {
        isLowerVolume = false;
    }

    public void ChangeVolume(string group, float value)
    {
        mixer.SetFloat(group,value);
    }

    void Update()
    {
        if (volumeSlider.value != 0)
        {
            if (!isLowerVolume)
                musicPlayer.volume = currentTrackVolume * volumeSlider.value;
            else
                musicPlayer.volume = (currentTrackVolume / 3) * volumeSlider.value;
        }
        else
        {
            musicPlayer.volume = 0;
        }
    }
}
