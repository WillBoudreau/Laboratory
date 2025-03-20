using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxButton : MonoBehaviour
{
    [Header("Box Button Controls")]
    [SerializeField] private GameObject[] objectToControl;//The object the button controls
    [SerializeField] private AudioClip buttonSound;//The sound the button makes
    [SerializeField] private AudioSource audioSource;//The audio source for the button

    void OnTriggerEnter(Collider other)
    {
        PlayButtonSound();
        foreach(GameObject obj in objectToControl)
        {
            if(other.GetComponent<Collider>().tag == "Box")
            {
                //If the object to control is a door
                if(obj.tag == "Door")
                {
                    obj.GetComponent<DoorBehaviour>().OpenThisDoor();
                }
                //If the object to control is a moving platform
                else if(obj.tag == "Platform")
                {
                    obj.GetComponent<MovingPlatform>().canMove = true;
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        foreach(GameObject obj in objectToControl)
        {
            if(other.GetComponent<Collider>().tag == "Box")
            {
                //If the object to control is a door
                if(obj.tag == "Door")
                {
                    obj.GetComponent<DoorBehaviour>().CloseThisDoor();
                }
                //If the object to control is a moving platform
                else if(obj.tag == "Platform")
                {
                    obj.GetComponent<MovingPlatform>().canMove = false;
                }
            }
        }
    }
    /// <summary>
    /// Play the button sound
    /// </summary>
    void PlayButtonSound()
    {
        audioSource.PlayOneShot(buttonSound);
    }
}
