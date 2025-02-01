using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;//The door object
    /// <summary>
    /// Open the door when the player enters the trigger
    /// </summary>
    void OpenDoor()
    {
        door.GetComponent<DoorBehaviour>().OpenThisDoor();
    }
    /// <summary>
    /// Close the door when the player exits the trigger
    /// </summary>
    void CloseDoor()
    {
        door.GetComponent<DoorBehaviour>().CloseThisDoor();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
           OpenDoor();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            CloseDoor();
        }
    }
}
