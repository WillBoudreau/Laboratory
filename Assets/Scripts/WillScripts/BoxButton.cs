using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxButton : MonoBehaviour
{
    [Header("Box Button Controls")]
    [SerializeField] private GameObject objectToControl;//The object the button controls

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Collider>().tag == "Box")
        {
            //If the object to control is a door
            if(objectToControl.tag == "Door")
            {
                objectToControl.GetComponent<DoorBehaviour>().OpenThisDoor();
            }
            //If the object to control is a moving platform
            else if(objectToControl.tag == "Platform")
            {
                objectToControl.GetComponent<MovingPlatform>().canMove = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Collider>().tag ==  "Box")
        {
            //If the object is a moving platform
            if(objectToControl.gameObject.tag == "Platform")
            {
                objectToControl.GetComponent<MovingPlatform>().canMove = false;
            }
            //If the object is a door
            else if(objectToControl.gameObject.tag == "Door")
            {
                objectToControl.GetComponent<DoorBehaviour>().CloseThisDoor();
            }  
        } 
    }
}
