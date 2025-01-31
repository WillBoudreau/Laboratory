using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LeverBehavior : MonoBehaviour
{
    [Header("Lever Controls")]
    [SerializeField] private GameObject objectToControl;//The object the lever controls
    private bool playerInRange = false;
    /// <summary>
    /// Activate the lever
    /// </summary>
    void ActivateLever()
    {
        //If the object to control is a door
        if(objectToControl.tag == "Door")
        {
           objectToControl.GetComponent<DoorBehaviour>().OpenThisDoor();
        }
        //If the object to control is a moving platform
        else if(objectToControl.tag == "MovingPlatform")
        {
           objectToControl.GetComponent<MovingPlatform>().canMove = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Collider>().tag == "Player")
        {
            Debug.Log("Player Triggered Lever");
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Collider>().tag == "Player")
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        //If the player is in range and presses the E key
        if(playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("Player Activated Lever");
            ActivateLever();
        }
    }
}
