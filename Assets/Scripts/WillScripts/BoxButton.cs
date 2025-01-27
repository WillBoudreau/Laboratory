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
            objectToControl.GetComponent<MovingPlatform>().canMove = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Collider>().tag ==  "Box")
        {
            objectToControl.GetComponent<MovingPlatform>().canMove = false;
        }   
    }
}
