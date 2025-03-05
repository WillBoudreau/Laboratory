using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For 3D Sound only!
/// </summary>
public class SFXTrigger : MonoBehaviour
{
    public AudioSource source3D;
    public AudioClip clip;
    public AudioClip clip2;
    // Start is called before the first frame update
    void Start()
    {
        source3D = gameObject.GetComponent<AudioSource>();
        source3D.loop = false;
    }

    public void PlaySFX(int clipNumber)
    {
        switch(clipNumber)
        {
            case 1:
                source3D.clip = clip;
                source3D.Play();
                break;
            case 2:
                source3D.clip = clip2;
                source3D.Play();
                break;
            default:
                source3D.PlayOneShot(clip);
                break;
        }
        
    }
}
