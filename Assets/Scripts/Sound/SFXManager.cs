using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Properties")]
    public AudioSource source2D;
    public AudioSource interactableSource2D;
    public AudioSource enviromentSource2D;
    [Header("2D sounds")]
    public AudioClip buttonPress;
    public List<AudioClip> movingPlatformSFX;
    public AudioClip metalStep;
    public AudioClip leverSFX;
    public AudioClip hurtSFX1;
    public AudioClip hurtSFX2;
    public AudioClip jumpSFX;
    public AudioClip boxDispenserSFX;
    public List<AudioClip> interactableSFX;
    public List<AudioClip> enviromentSFX;
    
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
        if(interactableSFX.Contains(clip))
        {
            interactableSource2D.clip = clip;
            interactableSource2D.loop = isLooping;
            interactableSource2D.Play();
        }
        else if(enviromentSFX.Contains(clip))
        {
            enviromentSource2D.clip = clip;
            enviromentSource2D.loop = isLooping;
            enviromentSource2D.Play();
        }
        else
        {
            source2D.clip = clip;
            source2D.loop = isLooping;
            source2D.Play();
        }
    }
}
