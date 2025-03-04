using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Properties")]
    public AudioSource source2D;
    [Header("2D sounds")]
    public AudioClip buttonPress;
    public AudioClip metalStep;
    public AudioClip leverSFX;
    public AudioClip hurtSFX1;
    public AudioClip hurtSFX2;
    public AudioClip jumpSFX;

    void Start()
    {
        source2D = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Takes in SFX to play, and a bool to determine looping. 
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="isLooping"></param>
    public void Player2DSFX(AudioClip clip, bool isLooping)
    {
        source2D.clip = clip;
        source2D.loop = isLooping;
        source2D.Play();
    }
}
