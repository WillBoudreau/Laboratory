using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    /// <summary>
    /// The type of button and what it controls
    /// </summary>
    public enum ButtonType
    {
        Box//The button controls a box
    }
    [Header("Button Settings")]
    [SerializeField] private ButtonType buttonType;//The type of button
    [SerializeField] private GameObject objectToControl;//The object the button controls
    void Start()
    {
        //If the object to control is null, find the suitable object to control
        if(objectToControl == null)
        {
            //If the button is a box type button
            if(buttonType == ButtonType.Box)
            {
                objectToControl = GameObject.FindGameObjectWithTag("Platform");
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //If the button is a box type button
        if(buttonType == ButtonType.Box)
        {
            //If the object is a box
            if(other.gameObject.tag == "Box")
            {
                //If the box is on the button, the platform cannot move
                //Find the MovingPlatform script and set canMove to false
                objectToControl.GetComponent<MovingPlatform>().canMove = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        //If the button is a box type button
        if(buttonType == ButtonType.Box)
        {
            //If the object is a box
            if(other.gameObject.tag == "Box")
            {
                //If the box is off the button, the platform can move
                //Find the MovingPlatform script and set canMove to true
                objectToControl.GetComponent<MovingPlatform>().canMove = false;
            }
        }
    }
}
