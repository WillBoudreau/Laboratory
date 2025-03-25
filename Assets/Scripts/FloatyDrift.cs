using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyDrift : MonoBehaviour
{
    public float speed = 1f; // Speed of horizontal movement
    public float resetPosition = -10f; // Position where the object resets
    public float startPosition = 10f; // Starting position when reset
    public float floatSpeed = 0.5f; // Vertical floating speed
    public float floatAmount = 0.5f; // Vertical floating range
    public float rotationSpeed = 10f; // Speed of rotation

    private Vector3 startPos;
    private float timeOffset;

    void Start()
    {
        startPos = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2f); // Offset for variation
    }

    void Update()
    {
        // Move continuously in one direction (leftward)
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Apply floaty vertical motion
        float y = Mathf.Sin(Time.time * floatSpeed + timeOffset) * floatAmount;
        transform.position = new Vector3(transform.position.x, startPos.y + y, transform.position.z);

        // Apply slow rotation
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Reset position when offscreen
        if (transform.position.x <= resetPosition)
        {
            transform.position = new Vector3(startPosition, transform.position.y, transform.position.z);
        }
    }
}