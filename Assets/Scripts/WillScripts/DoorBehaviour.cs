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
    [SerializeField] private float delay = 1.5f;//The delay before the door closes
    public bool isOpen = false;//If the door is open
    [SerializeField] private int targetIndex = 0;
    private SFXTrigger sFXTrigger;
    public bool canOpen;

    void Awake()
    {
        sFXTrigger = gameObject.GetComponent<SFXTrigger>();
        canOpen = true;
    }
    /// <summary>
    /// Move the door to the open position
    /// </summary>
    public void OpenThisDoor()
    {
        if(canOpen)
        {
            targetIndex = 1;
            if(sFXTrigger != null  && !sFXTrigger.source3D.isPlaying && !isOpen)
            {
                sFXTrigger.PlaySFX(1);
            }
            isOpen = true;
        }
    }
    /// <summary>
    /// Close the door once the player leaves the trigger
    /// </summary>
    public void CloseThisDoor()
    {
        if(canOpen)
        {
            targetIndex = 0;
            if(sFXTrigger != null && !sFXTrigger.source3D.isPlaying && isOpen  && canOpen)
            {
                sFXTrigger.PlaySFX(2);
            }
            isOpen = false;
        }
    }
    void Update()
    {
        //If the Door is unpowered, automatically move the door to the next position
        if(doorType == DoorType.UnPowered && canOpen)
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
        else if(doorType == DoorType.Powered && canOpen)
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
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            StartCoroutine(OpenAndCloseDoor(other, delay));
        }
        else if(other.gameObject.tag == "Player")
        {
            if(doorType == DoorType.Powered && canOpen)
            {
                if(!isOpen)
                {
                    OpenThisDoor();
                }
                else if(isOpen && targetIndex == 1)
                {
                    CloseThisDoor();
                }
            }
        }
    }
    /// <summary>
    /// When the door comes into contact with a box, open the door and then close it after a delay
    /// </summary>
    /// <param name="other">The other collider</param>
    /// <param name="delay">The delay before the door closes</param>
    /// <returns></returns>
    public IEnumerator OpenAndCloseDoor(Collider other, float delay)
    {
        if(other.gameObject.tag == "Box")
        {
            OpenThisDoor();
            yield return new WaitForSeconds(delay);
            CloseThisDoor();
        }
    }
}
