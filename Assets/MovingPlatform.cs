using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// Settings for the platform.
    /// speed modifies the speed that the platform moves between positions
    /// positions are the positions that the platform moves between
    /// currentPOS is the index of the current position that the platform is moving towards
    /// distance is the distance the platform enters and then moves towards the next
    /// </summary>
public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] float speed;
    [Header("Platform Positions")]
    [SerializeField] Transform[] positions = new Transform[2];
    [SerializeField] int currentPos;
    [SerializeField] float distance = 0.1f;

    void Update()
    {
        Vector3 TargetPOS = positions[currentPos].position;

        transform.position = Vector3.MoveTowards(transform.position, TargetPOS, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position,TargetPOS) <= distance)
        {
            currentPos = (currentPos + 1) % positions.Length;
        }
    }
}
