using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private GameObject[] objectsToControl;//The object the button controls
    [SerializeField] private int objectsOnButton = 0;//The number of objects on the button
    [Header("Audio Settings")]
    [SerializeField] private SFXManager sFXManager;//The SFX manager
    void Awake()
    {
        sFXManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        //For each object in the array
        foreach(GameObject obj in objectsToControl)
        {
            Debug.Log(obj.tag);
            //If the object that enters the trigger is a box or the player
            if(other.gameObject.tag == "Box" || other.gameObject.tag == "Player")
            {
                objectsOnButton++;
                sFXManager.Player2DSFX(sFXManager.buttonPress,false);
                Debug.Log("Box or Player can interact with button");
                //If the object is a door, open it
                if(obj.tag == "Door")
                {
                    Debug.Log("Door can open");
                    obj.GetComponent<DoorBehaviour>().OpenThisDoor();
                }
                //If the object is a platform, allow it to move
                else if(obj.tag == "Platform")
                {
                    Debug.Log("Platform can move");
                    obj.GetComponent<MovingPlatform>().canMove = true;  
                }
                else if(obj.tag == "Receiver")
                {
                    Debug.Log(("firing laser"));
                    obj.GetComponent<LaserEmitter>().FireLaser();
                }
                else if(obj.tag == "BoxDispenser")
                {
                    Debug.Log(("dispensing"));
                    obj.GetComponent<BoxSpawner>().SpawnBox();
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        //For each object in the array
        foreach(GameObject obj in objectsToControl)
        {
            //If the object that enters the trigger is a box or the player
            if(other.gameObject.tag == "Box" || other.gameObject.tag == "Player")
            {
                objectsOnButton--;
                if(objectsOnButton <= 0)
                {
                    objectsOnButton = 0;
                    //If the object is a door, close it
                    if(obj.tag == "Door")
                    {
                        obj.GetComponent<DoorBehaviour>().CloseThisDoor();
                    }
                    //If the object is a platform, stop it from moving
                    else if(obj.tag == "Platform")
                    {
                        obj.GetComponent<MovingPlatform>().canMove = false;
                        obj.GetComponent<MovingPlatform>().PlayPlatformSound(obj.GetComponent<MovingPlatform>().deactivateSoundIndex); 
                    }
                    else if(obj.tag == "Receiver")
                    {
                        obj.GetComponent<LaserEmitter>().DisableLaser();
                    }
                }
            }
        }
    }
    // /// <summary>
    // /// Play the button sound
    // /// </summary>
    // void PlayButtonSound()
    // {
    //     audioSource.PlayOneShot(buttonSound);
    // }
}
