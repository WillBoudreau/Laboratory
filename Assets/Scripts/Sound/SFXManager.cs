using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Properties")]
    public AudioSource source2D;
    public AudioSource interactableSource2D;
    public AudioSource enviromentSource2D;
    public AudioSource playerSound2D;
    public AudioSource playerWalkSource;
    public AudioSource uiSource;
    [Header("2D sounds")]
    public AudioClip buttonPress;
    public AudioClip metalStep;
    public AudioClip leverSFX;
    public AudioClip hurtSFX1;
    public AudioClip hurtSFX2;
    public AudioClip jumpSFX;
    public AudioClip boxDispenserSFX;
    public AudioClip boxLandSFX;
    public AudioClip damageSFX;
    public AudioClip buttonHoverSFX;
    public AudioClip buttonClickSFX;
    public AudioClip buttonClick2SFX;
    [Header("3D sounds")]
    public AudioClip ambientSFX;
    [Header("Lists")]
    public List<AudioClip> buttonSFX;
    public List<AudioClip> movingPlatformSFX;
    public List<AudioClip> interactableSFX;
    public List<AudioClip> enviromentSFX;
    public List<AudioClip> playerHurtSFX;
    public List<AudioClip> playerDeathSFX;
    public List<AudioClip> uISFX;
    
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
            if(interactableSource2D.isPlaying)
            {
                interactableSource2D.Stop();
            }
            interactableSource2D.clip = clip;
            interactableSource2D.loop = isLooping;
            interactableSource2D.Play();
        }
        else if(buttonSFX.Contains(clip))
        {
            if(interactableSource2D.isPlaying)
            {
                interactableSource2D.Stop();
            }
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
        else if(uISFX.Contains(clip))
        {
            uiSource.clip = clip;
            uiSource.loop = isLooping;
            uiSource.PlayOneShot(clip,0.5f);
        }
        else if(playerDeathSFX.Contains(clip) || playerHurtSFX.Contains(clip))
        {
            playerSound2D.clip = clip;
            playerSound2D.loop = isLooping;
            playerSound2D.Play();
        }
        else if(clip == metalStep)
        {
            playerWalkSource.clip = clip;
            playerWalkSource.loop = isLooping;
            playerWalkSource.Play();
        }
        else
        {
            source2D.clip = clip;
            source2D.loop = isLooping;
            source2D.Play();
        }
    }
}
