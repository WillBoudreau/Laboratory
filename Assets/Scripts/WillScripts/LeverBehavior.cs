using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LeverBehavior : MonoBehaviour
{
    [Header("Lever Controls")]
    [SerializeField] private GameObject[] objectToControl;//The object the lever controls
    [SerializeField] private bool setToDamaged;//Set the lever to damaged
    private bool playerInRange = false;
    /// <summary>
    /// Activate the lever
    /// </summary>
    public void ActivateLever()
    {
        Debug.Log("Lever Activated");
        foreach(GameObject obj in objectToControl)
        {
            Debug.Log(obj.tag);
            switch(obj.tag)
            {
                case "Door":
                    if (obj.GetComponent<DoorBehaviour>().isOpen)
                    {
                        obj.GetComponent<DoorBehaviour>().CloseThisDoor();
                    }
                    else
                    {
                        obj.GetComponent<DoorBehaviour>().OpenThisDoor();
                    }
                    break;
                case "Platform":
                    if(obj.GetComponent<MovingPlatform>().canMove == true)
                    {
                        obj.GetComponent<MovingPlatform>().canMove = false;
                    }
                    else
                    {
                        obj.GetComponent<MovingPlatform>().canMove = true;
                    }
                    break;
                case "Elevator":
                    obj.GetComponent<ElevatorBehaviour>().canMove = true;
                    break;
                case "Reflector":
                    obj.GetComponent<ReflectorBehaviour>().StartCoroutine("RotateReflectorCoroutine");
                    break;
                case "Receiver":
                    if(!setToDamaged)
                    {
                        obj.GetComponent<LaserEmitter>().FireLaser();
                    }
                    else
                    {
                        obj.GetComponent<LaserEmitter>().SwitchLaserType();
                    }
                    break;
            }
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
