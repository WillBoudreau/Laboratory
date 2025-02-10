using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingPlatform : MonoBehaviour 
{
    private enum MovementType // The type of movement the platform will have between target positions
    {
        BackAndForth, // Move between target positions in the pattern 1,2,3,2,1,2,3...
        Loop // Move between target positions in the pattern 1,2,3,1,2,3,1...
    }; 

    [Header("Class calls")]
    [SerializeField] private Singleton singleton; // The singleton object
    
    [Header("Platform Movement Settings")]
    [SerializeField] private float speed; // Speed of the platform
    [SerializeField] public bool canMove = true; // if the platform can move
    [SerializeField] private float movementPauseTime = 0.5f; // When the moving platform reachs its destination, wait this long before moving again
    [SerializeField] private MovementType movementType; // The type of movement the platform will have

    [Header("Platform Positions")]
    [SerializeField] private List<Transform> targetPositions = new List<Transform>(); // List of target positions the platform moves to
    [SerializeField] private int previousTargetIndex; // The previous target position index of the platform
    [SerializeField] private int currentTargetIndex; // Current target position index of the platform
    [SerializeField] private float distanceToHitTarget = 0.1f; // The point where the platform will move to the next position

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
        Vector3 targetPos = targetPositions[currentTargetIndex].position;

        // Move towards the target position
        previousPosition = transform.position; // Update the previous position as platform moves
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Check if platform has reached the target position
        if (Vector3.Distance(transform.position, targetPos) <= distanceToHitTarget)
        {
            // Set previous target index
            previousTargetIndex = currentTargetIndex;

            // Setup movement pause timer
            movementPauseTimer = movementPauseTime;

            switch (movementType)
            {
                case MovementType.BackAndForth:

                    // If the platform is at the last position, reverse the direction
                    if (currentTargetIndex == targetPositions.Count - 1)
                    {
                        movingForward = false;
                    }

                    // If the platform is at the first position, reverse the direction
                    if (currentTargetIndex == 0)
                    {
                        movingForward = true;
                    }

                break;

                case MovementType.Loop:

                    // If the platform is at the last position and its moving forward, go back to target position 0 or 1
                    if (currentTargetIndex == targetPositions.Count - 1 && movingForward == true)
                    {
                        currentTargetIndex = -1;
                    }

                    // If the platform is at the last position and its moving backward, go back to target position at the end of the List
                    if (currentTargetIndex == 0 && movingForward == false)
                    {
                        currentTargetIndex = targetPositions.Count;
                    }

                break;
            }

            // Move to the next position based on the direction
            if (movingForward)
            {
                currentTargetIndex++;
            }
            else
            {
                currentTargetIndex--;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");

        // If the object is the player, make the player a child of the platform
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }

        if(other.gameObject.tag == "Door")
        {
            

            if(Vector3.Distance(transform.position, previousPosition) <= 1f)
            {
                // Reverse moving direction
                if (movingForward)
                {
                    movingForward = false;
                }
                else
                {
                    movingForward = true;
                }

                int previousTargetIndexTemp = previousTargetIndex;
                previousTargetIndex = currentTargetIndex;
                currentTargetIndex = previousTargetIndexTemp;
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