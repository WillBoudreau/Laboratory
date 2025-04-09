using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCrushTrigger : MonoBehaviour
{
    [Header("Class calls")]
    [SerializeField] private MovingPlatform movingPlatform; // The moving platform
    [SerializeField] private float movementPauseTime = 0f; // When the moving platform reachs its destination, wait this long before moving again
    [SerializeField] private float originalMovementPauseTime = 0.5f; // The original movement pause time
    void Start()
    {
        originalMovementPauseTime = movingPlatform.movementPauseTime; // Set the original movement pause time to the current movement pause time
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            movingPlatform.movementPauseTime = movementPauseTime; // Set the movement pause time to 0
            movingPlatform.SwitchDirection(); // Switch the direction of the platform
            movingPlatform.movementPauseTime = originalMovementPauseTime; // Set the movement pause time back to the original value
        }
    }
}
