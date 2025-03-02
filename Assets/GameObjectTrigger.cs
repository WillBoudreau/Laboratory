using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTrigger : MonoBehaviour
{
    [Header("GameObject Settings")]
    [SerializeField] private GameObject[] objectToTrigger; // The object to trigger
    [SerializeField] private bool triggerOnce; // If the trigger should only be activated once
    private bool triggered; // If the trigger has been activated

    void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            foreach (GameObject obj in objectToTrigger)
            {
                if (other.GetComponent<Collider>().tag == "Player")
                {
                    // If the object to control is a door
                    if (obj.tag == "Door")
                    {
                        obj.GetComponent<DoorBehaviour>().OpenThisDoor();
                    }
                    // If the object to control is a moving platform
                    else if (obj.tag == "Platform")
                    {
                        obj.GetComponent<MovingPlatform>().canMove = true;
                    }
                }
            }
            if (triggerOnce)
            {
                triggered = true;
            }
        }
    }
}
