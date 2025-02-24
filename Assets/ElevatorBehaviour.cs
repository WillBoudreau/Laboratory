using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBehaviour : MonoBehaviour
{
    [Header("Elevator Movement Settings")]
    [SerializeField] private float speed = 3; // Speed of the platform
    public bool canMove; // if the platform can move
    [SerializeField] private float movementPauseTime = 0.5f; // When the moving platform reachs its destination, wait this long before moving again
    [SerializeField] private GameObject[] targetPositions; // List of target positions the platform moves to


    void Update()
    {
        // If the platform can move, move to the next position
        if (canMove)
        {
            MoveToNextPosition();
        }
    }
    /// <summary>
    /// Move the elevator to the next position
    /// </summary>
    void MoveToNextPosition()
    {
        // If the movement pause is not in effect/finished, move to next position
        if (canMove)
        {
            // Move the platform to the next position
            transform.position = Vector3.MoveTowards(transform.position, targetPositions[1].transform.position, speed * Time.deltaTime);

            // If the platform has reached the target position
            if (Vector3.Distance(transform.position, targetPositions[1].transform.position) < 0.01f)
            {
                // Stop the platform from moving
                canMove = false;

                // Start the movement pause timer
                StartCoroutine(MovementPause());
            }
        }
    }
    /// <summary>
    /// Pause the movement of the platform
    /// </summary>
    /// <returns></returns>
    IEnumerator MovementPause()
    {
        // Wait for the movement pause time
        yield return new WaitForSeconds(movementPauseTime);

        // Start moving the platform again
        canMove = true;
    }
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         other.transform.SetParent(transform,true);
    //     }
    // }
    // void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         other.transform.SetParent(null);
    //     }
    // }
}
