using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyButton : MonoBehaviour
{
    [Header("Heavy Button Settings")]
    [SerializeField] private GameObject[] objectsToControl;//The object the button controls
    [SerializeField] private float massThreshold;//The mass threshold for the object to control
    [Header("Audio Settings")]
    [SerializeField] private SFXManager sFXManager;//The SFX manager
    void Awake()
    {
        sFXManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        sFXManager.Player2DSFX(sFXManager.buttonPress,false);
    }
    void OnTriggerStay(Collider other)
    {
        foreach(GameObject objectToControl in objectsToControl)
        {
            if(other.gameObject.GetComponent<Rigidbody>().mass > massThreshold)
            {
                //If the object is a moving platform
                if(objectToControl.gameObject.tag == "Platform")
                {
                    objectToControl.GetComponent<MovingPlatform>().canMove = true;
                }
                //If the object is a door
                else if(objectToControl.gameObject.tag == "Door")
                {
                    objectToControl.GetComponent<DoorBehaviour>().OpenThisDoor();
                }
                //If the object is a box dispenser
                else if(objectToControl.gameObject.tag == "BoxDispenser")
                {
                    objectToControl.GetComponent<BoxSpawner>().SpawnBox();
                }
                //if the object is a reflector
                else if(objectToControl.gameObject.tag == "Reflector")
                {
                    objectToControl.GetComponent<ReflectorBehaviour>().canRotate = false;
                }
            }
            else if(other.gameObject.GetComponent<Rigidbody>() == null)
            {
                return;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        foreach(GameObject objectToControl in objectsToControl)
        {
            if(other.gameObject.GetComponent<Rigidbody>().mass > massThreshold)
            {
                if(objectToControl.gameObject.tag == "Platform")
                {
                    objectToControl.GetComponent<MovingPlatform>().canMove = false;
                }
                else if(objectToControl.gameObject.tag == "Door")
                {
                    objectToControl.GetComponent<DoorBehaviour>().CloseThisDoor();
                }
                else if(objectToControl.gameObject.tag == "Reflector")
                {
                    objectToControl.GetComponent<ReflectorBehaviour>().canRotate = true;
                    objectToControl.GetComponent<ReflectorBehaviour>().StartCoroutine("RotateReflectorCoroutine");
                }
            }
        }
    }
}
