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
                    Debug.Log(obj.GetComponent<MovingPlatform>().canMove + "1");
                    if(obj.GetComponent<MovingPlatform>().canMove == false)
                    {
                        obj.GetComponent<MovingPlatform>().canMove = true;
                    }
                    else
                    {
                        obj.GetComponent<MovingPlatform>().canMove = false;
                        break;
                    }
                    Debug.Log(obj.GetComponent<MovingPlatform>().canMove + "2");
                    break;
                case "Elevator":
                    obj.GetComponent<ElevatorBehaviour>().canMove = true;
                    break;
                case "Reflector":
                    obj.GetComponent<ReflectorBehaviour>().canRotate = true;
                    obj.GetComponent<ReflectorBehaviour>().StartCoroutine("RotateReflectorCoroutine");
                    break;
                case "Receiver":
                    if(!setToDamaged)
                    {
                        obj.GetComponent<LaserEmitter>().isButtonActivated = false;
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
            if(Keyboard.current.eKey.isPressed)
            {
                ActivateLever();
            }
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
        // //If the player is in range and presses the E key
        // if(playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        // {
        //     Debug.Log("Player Activated Lever");
        //     ActivateLever();
        // }
    }
}
