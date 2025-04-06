using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;//The door object
    public SFXTrigger sFXTrigger; //Needed for 3D sfx


    void Awake()
    {
        sFXTrigger = gameObject.GetComponent<SFXTrigger>();
    }
    /// <summary>
    /// Open the door when the player enters the trigger
    /// </summary>
    void OpenDoor()
    {
        door.GetComponent<DoorBehaviour>().OpenThisDoor();
        sFXTrigger.PlaySFX(1);
    }
    /// <summary>
    /// Close the door when the player exits the trigger
    /// </summary>
    void CloseDoor()
    {
        door.GetComponent<DoorBehaviour>().CloseThisDoor();
        sFXTrigger.PlaySFX(2);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && door.GetComponent<DoorBehaviour>().canOpen)
        {
           OpenDoor();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player" && door.GetComponent<DoorBehaviour>().canOpen)
        {
            CloseDoor();
        }
    }
}
