using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyButton : MonoBehaviour
{
    [Header("Heavy Button Settings")]
    [SerializeField] private GameObject objectToControl;//The object the button controls
    [SerializeField] private float massThreshold;//The mass threshold for the object to control
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Rigidbody>().mass > massThreshold)
        {
            Debug.Log("Heavy object on button");
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
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Rigidbody>().mass > massThreshold)
        {
            if(objectToControl.gameObject.tag == "Platform")
            {
                objectToControl.GetComponent<MovingPlatform>().canMove = false;
            }
        }
    }
}
