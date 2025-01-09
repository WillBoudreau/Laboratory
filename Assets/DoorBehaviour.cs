using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] positions = new GameObject[2];//The positions the door can move to
    [SerializeField] private float speed;//The speed of the door
    [SerializeField] private float distance = 0.1f;//The point where the door will move to the next position
    [SerializeField] private bool isOpen = false;//If the door is open
    int targetIndex = 0;
    /// <summary>
    /// Move the door to the open position
    /// </summary>
    public void OpenThisDoor()
    {
       targetIndex = 0;
       isOpen = true;
    }
    /// <summary>
    /// Close the door once the player leaves the trigger
    /// </summary>
    public void CloseThisDoor()
    {
        targetIndex = 1;
        isOpen = false;
    }
    void Update()
    {
        //If the door is open, move the door to the open position
        if(isOpen)
        {
            MoveDoor();
        }
        //If the door is closed, move the door to the closed position
        else if(!isOpen && targetIndex == 1)
        {
            MoveDoor();
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
}
