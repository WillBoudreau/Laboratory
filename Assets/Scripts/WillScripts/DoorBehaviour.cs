using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private enum DoorType {UnPowered, Powered};//The type of door
    [SerializeField] private DoorType doorType;//The type of door
    [Header("Door Controls")]
    [SerializeField] private GameObject[] positions = new GameObject[2];//The positions the door can move to,FOR INSPECTOR Closed first then Open
    [SerializeField] private float speed;//The speed of the door
    [SerializeField] private float distance = 0.1f;//The point where the door will move to the next position
    public bool isOpen = false;//If the door is open
    [SerializeField] private int targetIndex = 0;
    private SFXTrigger sFXTrigger;

    void Awake()
    {
        sFXTrigger = gameObject.GetComponent<SFXTrigger>();
    }
    /// <summary>
    /// Move the door to the open position
    /// </summary>
    public void OpenThisDoor()
    {
       targetIndex = 1;
       isOpen = true;
       if(sFXTrigger != null)
       {
            sFXTrigger.PlaySFX(1);
       }
    }
    /// <summary>
    /// Close the door once the player leaves the trigger
    /// </summary>
    public void CloseThisDoor()
    {
        targetIndex = 0;
        isOpen = false;
        if(sFXTrigger != null)
       {
            sFXTrigger.PlaySFX(2);
       }
    }
    void Update()
    {
        //If the Door is unpowered, automatically move the door to the next position
        if(doorType == DoorType.UnPowered)
        {
           //If the door is open, move the door to the open position
            if(!isOpen)
            {
                MoveDoor();
            }
            //If the door is closed, move the door to the closed position
            else if(isOpen && targetIndex == 1)
            {
                MoveDoor();
            }
        }
        //If the door is powered, only move the door when the player powers the door
        else if(doorType == DoorType.Powered)
        {
            if(!isOpen)
            {
                MoveDoor();
            }
            else if(isOpen && targetIndex == 1)
            {
                MoveDoor();
            }
        } 
    }
    /// <summary>
    /// Move the door to the next position
    /// </summary>
    /// <param name="targetIndex">The index of the target position</param>
    void MoveDoor()
    {
        // Move the door to the next position
        Vector3 targetPOS = positions[targetIndex].transform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPOS, speed * Time.deltaTime);
    }
    void OntTriggerEnter(Collision other)
    {
        if(other.gameObject.tag == "Box")
        {
            if(!isOpen)
            {
                OpenThisDoor();
            }
            else
            {
                CloseThisDoor();
            }
        }
    }
}
