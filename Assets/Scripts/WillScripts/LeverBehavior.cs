using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LeverBehavior : MonoBehaviour
{
    [Header("Class calls")]
    [SerializeField] private SFXManager sFXManager;//The SFX manager
    [Header("Lever Controls")]
    [SerializeField] private GameObject[] objectToControl;//The object the lever controls
    [SerializeField] private bool setToDamaged;//Set the lever to damaged
    [Header("Debug Controls")]
    [SerializeField] private bool debugMode;//Debug mode
    [SerializeField] private bool debugActivate;//Debug activate the lever
    [SerializeField] private bool debugDeactivate;//Debug deactivate the lever
    private bool playerInRange = false;
    public Animator animator;
    void Awake()
    {
        sFXManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
    }
    /// <summary>
    /// Activate the lever
    /// </summary>
    public void ActivateLever()
    {
        if(animator != null)
        {
            animator.SetTrigger("changeState");
        }
        sFXManager.Player2DSFX(sFXManager.leverSFX,false);
        Debug.Log("Lever Activated");
        foreach(GameObject obj in objectToControl)
        {
            Debug.Log(obj.tag);
            switch(obj.tag)
            {
                //Open or close the door
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
                //Move the platform
                case "Platform":
                    Debug.Log(obj.GetComponent<MovingPlatform>().canMove + "1");
                    if(obj.GetComponent<MovingPlatform>().canMove == false)
                    {
                        obj.GetComponent<MovingPlatform>().SetMovementStatus(true);
                    }
                    else
                    {
                        obj.GetComponent<MovingPlatform>().SetMovementStatus(false);
                        break;
                    }
                    Debug.Log(obj.GetComponent<MovingPlatform>().canMove + "2");
                    break;
                //Move the elevator
                case "Elevator":
                    obj.GetComponent<ElevatorBehaviour>().canMove = true;
                    break;
                //Rotate the reflector
                case "Reflector":
                    //obj.GetComponent<ReflectorBehaviour>().canRotate = true;
                    obj.GetComponent<ReflectorBehaviour>().StartCoroutine("RotateReflectorCoroutine");
                    break;
                //Switch the laser type
                case "Receiver":
                    if(!setToDamaged)
                    {
                        obj.GetComponent<LaserEmitter>().isButtonActivated = false;
                        obj.GetComponent<LaserEmitter>().PlaySparkSound();
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
    void OnDrawGizmos()
    {
        if (debugMode)
        {
            if(sFXManager == null)
            {
                return;
            }
            // Draw lines from the lever to the objects it controls
            Gizmos.color = Color.red;
            foreach (GameObject obj in objectToControl)
            {
                if (obj != null)
                {
                    Gizmos.DrawLine(transform.position, obj.transform.position);
                }
            }
            if (debugActivate)
            {
                ActivateLever();
                debugActivate = false;
            }
            if (debugDeactivate)
            {
                //DeactivateLever();
                debugDeactivate = false;
            }
        }
    }

    // /// <summary>
    // /// Play the lever sound effect
    // /// </summary>
    // void PlayLeverSound()
    // {
    //     audioSource.PlayOneShot(leverSound);
    // }
}
