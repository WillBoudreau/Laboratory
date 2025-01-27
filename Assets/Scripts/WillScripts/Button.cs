using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private GameObject objectToControl;//The object the button controls
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           //If the box is on the button, the platform cannot move
           //Find the MovingPlatform script and set canMove to false
           objectToControl.GetComponent<MovingPlatform>().canMove = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //If the box is off the button, the platform can move
            //Find the MovingPlatform script and set canMove to true
            objectToControl.GetComponent<MovingPlatform>().canMove = false;
        }
    }
}
