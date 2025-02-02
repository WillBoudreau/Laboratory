using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPuzzlePlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private float moveSpeed = 5f;//The speed at which the platform moves
    [SerializeField] private Transform[] movePoints;//The points the platform moves between
    [SerializeField] private int currentPoint = 0;//The current point the platform is moving to
    [SerializeField] private bool canMove = true;//If the platform can move
    [SerializeField] private float distance = 0.1f;//The distance the platform needs to be from the point to move to the next point
    private Vector3 previousPosition;//The previous position of the platform
    private void Start()
    {
        previousPosition = transform.position;//Set the previous position to the current position
    }
    private void Update()
    {
        if(canMove)
        {
            MoveToNextPoint();
        }
    }
    private void MoveToNextPoint()
    {
        Vector3 targetPosition = movePoints[currentPoint].position;//The position the platform is moving to
        previousPosition = transform.position;//Set the previous position to the current position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);//Move the platform to the target position
        if(Vector3.Distance(transform.position, targetPosition) <= distance)//If the platform is close enough to the target position
        {
            currentPoint = (currentPoint + 1) % movePoints.Length;//Move to the next point
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            Destroy(other.gameObject);//Destroy the box
        }
    }
}
