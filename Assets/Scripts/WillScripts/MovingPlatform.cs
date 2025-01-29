using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] float speed;//Speed of the platform
    [SerializeField] public bool canMove = false;//if the platform can move
    [Header("Platform Positions")]
    [SerializeField] Transform[] positions = new Transform[2];//Array of positions the platform can move to
    [SerializeField] int currentPos;//Current position of the platform
    [SerializeField] float distance = 0.1f;//The point where the platform will move to the next position
    void Start()
    {
        canMove = false;
    }

    void Update()
    {
        //If the platform can move, move to the next position
        if(canMove)
        {
            MoveToNextPosition();
        }
    }
    /// <summary>
    /// Move the platform to the next position within the positions array
    /// </summary>
    void MoveToNextPosition()
    {
        // Move the platform to the next position
        Vector3 targetPOS = positions[currentPos].position;

        transform.position = Vector3.MoveTowards(transform.position, targetPOS, speed * Time.deltaTime);

        // If the platform has reached the next position, move to the next position
        if(Vector3.Distance(transform.position,targetPOS) <= distance)
        {
            currentPos = (currentPos + 1) % positions.Length;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //If the object is the player, make the player a child of the platform
        if(other.gameObject.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }
    void OnTriggerExit(Collider other)
    {
        //If the object is the player, remove the player as a child of the platform
        if(other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
