using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingPlatform : MonoBehaviour 
{
    [Header("Class calls")]
    [SerializeField] private Singleton singleton; // The singleton object
    
    [Header("Platform Movement Settings")]
    [SerializeField] private float speed; // Speed of the platform
    [SerializeField] public bool canMove = false; // if the platform can move
    [SerializeField] private float movementPauseTime = 0.5f; // When the moving platform reachs its destination, wait this long before moving again
    [SerializeField] private enum PlatformType {Constant, Limited}; // Type of platform movement
    [SerializeField] private PlatformType platformType; // The type of platform movement
    [SerializeField] private int platformMovementLimit; // The limit of platform movement
    [SerializeField] public int platformMovementTick; // The tick of platform movement

    [Header("Platform Positions")]
    [SerializeField] private Transform[] positions = new Transform[2]; // Array of positions the platform can move to
    [SerializeField] private int currentPos; // Current position of the platform
    [SerializeField] private float distance = 0.1f; // The point where the platform will move to the next position

    private Vector3 previousPosition; // Store the previous position of the platform
    private float movementPauseTimer = 0;

    void Start()
    {
        previousPosition = transform.position; // Initialize the previous position

        if(singleton == null)
        {
            singleton = FindObjectOfType<Singleton>();
        }
    }

    void Update()
    {
        // If the platform can move, move to the next position
        if (canMove)
        {
            // If the movement pause is not in effect/finished, move to next position
            if (movementPauseTimer <= 0)
            {
                // If the platform type is limited, check if the platform has reached the limit
                if(PlatformType.Limited == platformType)
                {
                    if(platformMovementTick <= platformMovementLimit)
                    {
                        MoveToNextPosition();
                    }
                    else if(platformMovementTick >= platformMovementLimit)
                    {
                        canMove = false;
                    }
                }
                else
                {
                    MoveToNextPosition();
                }
            }
            else
            {
                movementPauseTimer -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Move the platform to the next position within the positions array
    /// </summary>
    void MoveToNextPosition()
    {
        // Get the target position the platform is moving towards
        Vector3 targetPos = positions[currentPos].position;

        // Move towards the target position
        previousPosition = transform.position; // Update the previous position as platform moves
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Check if platform has reached the target position
        if (Vector3.Distance(transform.position, targetPos) <= distance)
        {
            // Setup movement pause timer
            movementPauseTimer = movementPauseTime;
            currentPos = (currentPos + 1) % positions.Length;
            // Increment the platform movement tick
            if(PlatformType.Limited == platformType)
            {
                platformMovementTick++;
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        // If the object is the player, make the player a child of the platform
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform,true);
        }
        else if(other.gameObject.tag == "Door")
        {
            if(Vector3.Distance(transform.position, previousPosition) <= 1f)
            {
                // If the platform is not moving, reverse the direction
                currentPos = (currentPos + 1) % positions.Length;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the object is the player, remove the player as a child of the platform
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(singleton.transform,true);
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     Log a message when the platform collides with another object
    //     Debug.Log("Platform collided with " + collision.gameObject.name);
    //     if(collision.gameObject != this.gameObject && collision.gameObject.tag != "Player")
    //     {
    //         if(Vector3.Distance(transform.position, previousPosition) <= 1f)
    //         {
    //             If the platform is not moving, reverse the direction
    //             currentPos = (currentPos + 1) % positions.Length;
    //         }
    //     }
    // }
}