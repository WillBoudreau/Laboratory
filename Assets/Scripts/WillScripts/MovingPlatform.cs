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
    [SerializeField] private enum MovementType {BackAndForth, ExtraPOS}; // The type of movement the platform will have
    [SerializeField] private MovementType movementType; // The type of movement the platform will have

    [Header("Platform Positions")]
    [SerializeField] private List<Transform> positions = new List<Transform>(); // List of positions the platform can move to
    [SerializeField] private int previousPOS; // The previous position of the platform
    [SerializeField] private int currentPos; // Current position of the platform
    [SerializeField] private float distance = 0.1f; // The point where the platform will move to the next position

    private Vector3 previousPosition; // Store the previous position of the platform
    private float movementPauseTimer = 0;
    private bool movingForward = true; // Direction of movement

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
                MoveToNextPosition();
            }
            else
            {
                movementPauseTimer -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Move the platform to the next position within the positions list
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

            // If the platform is at the last position, reverse the direction
            if (currentPos == positions.Count - 1)
            {
                movingForward = false;
            }
            // If the platform is at the first position, keep the direction
            else if (currentPos == 0)
            {
                movingForward = true;
            }

            // Move to the next position based on the direction
            if (movingForward)
            {
                currentPos++;
            }
            else
            {
                currentPos--;
            }
        }
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
                currentPos = (currentPos + 1) % positions.Count;
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
}