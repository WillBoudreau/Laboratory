using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingPlatform : MonoBehaviour 
{
    [Header("Platform Settings")]
    [SerializeField] float speed; // Speed of the platform
    [SerializeField] public bool canMove = false; // if the platform can move
    [Header("Platform Positions")]
    [SerializeField] Transform[] positions = new Transform[2]; // Array of positions the platform can move to
    [SerializeField] int currentPos; // Current position of the platform
    [SerializeField] float distance = 0.1f; // The point where the platform will move to the next position
    [SerializeField] LayerMask obstacleLayer; // Layer mask to specify which layers are considered obstacles

    [SerializeField] private Vector3 originalPosition; // Store the original position of the platform
    private Vector3 previousPosition; // Store the previous position of the platform

    void Start()
    {
        canMove = false;
        originalPosition = transform.position; // Initialize the original position
        previousPosition = transform.position; // Initialize the previous position
    }

    void Update()
    {
        // If the platform can move, move to the next position
        if (canMove)
        {
            MoveToNextPosition();
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
            currentPos = (currentPos + 1) % positions.Length;
        }
        else if (CheckPlatformInterference(targetPos))
        {
            // Move back to the previous position if an obstacle is detected
            //Vector3 direction = (previousPosition - transform.position).normalized;
            //transform.position += direction * speed * Time.deltaTime;
            currentPos = (currentPos + 1) % positions.Length;
        }
    }

    /// <summary>
    /// Check for any interference with the platform's movement
    /// </summary>
    bool CheckPlatformInterference(Vector3 targetPos)
    {
        // Check if there is any obstacle between the platform's current position and the target position
        return Physics.Linecast(transform.position, targetPos, obstacleLayer);
    }

    void OnTriggerEnter(Collider other)
    {
        // If the object is the player, make the player a child of the platform
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the object is the player, remove the player as a child of the platform
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
