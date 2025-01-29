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
            objectToControl.GetComponent<MovingPlatform>().canMove = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Rigidbody>().mass > massThreshold)
        {
            objectToControl.GetComponent<MovingPlatform>().canMove = false;
        }
    }
}
