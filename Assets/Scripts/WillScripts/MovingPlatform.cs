using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingPlatform : MonoBehaviour 
{
    [Header("Class calls")]
    [SerializeField] private Singleton singleton; // The singleton object
    
    [Header("Platform Movement Settings")]
    [SerializeField] float speed; // Speed of the platform
    [SerializeField] public bool canMove = false; // if the platform can move
    [SerializeField] private enum PlatformType {Constant, Limited}; // Type of platform movement
    [SerializeField] private PlatformType platformType; // The type of platform movement
    // The limit of platform movement
    [SerializeField] private int platformMovementLimit; // The limit of platform movement
    public int platformMovementTick; // The tick of platform movement
    [Header("Platform Positions")]
    [SerializeField] Transform[] positions = new Transform[2]; // Array of positions the platform can move to
    [SerializeField] int currentPos; // Current position of the platform
    [SerializeField] float distance = 0.1f; // The point where the platform will move to the next position
    private Vector3 previousPosition; // Store the previous position of the platform

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
            StartCoroutine(Timer());
            currentPos = (currentPos + 1) % positions.Length;
            // Increment the platform movement tick
            if(PlatformType.Limited == platformType)
            {
                platformMovementTick++;
            }
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2);
    }
    void OnTriggerEnter(Collider other)
    {
        // If the object is the player, make the player a child of the platform
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
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
            other.transform.parent = singleton.transform;
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