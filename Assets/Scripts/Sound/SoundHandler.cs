using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject soundPlayerPrefab;
    public Slider volumeSlider;

    public void PlaySound(AudioClip sound, float volume = 1, float spatialBlend = 0, Vector3? position = null)
    {
        GameObject soundPlayerGO = Instantiate(soundPlayerPrefab);

        if (position != null)
            soundPlayerGO.transform.position = (Vector3)position;

        soundPlayerGO.TryGetComponent<AudioSource>(out AudioSource soundPlayer);
        soundPlayer.clip = sound;
        soundPlayer.volume = volume;
        soundPlayer.spatialBlend = spatialBlend;

        if (volumeSlider.value != 0)
            soundPlayer.volume = soundPlayer.volume * volumeSlider.value;
        else
            soundPlayer.volume = 0;

        soundPlayer.GetComponent<AudioSource>().Play();
    }

}
