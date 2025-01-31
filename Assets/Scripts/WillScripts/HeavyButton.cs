using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToControl;//The object the button controls
    [SerializeField] private float massThreshold;
    void OnTriggerEnter(Collider other)
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
