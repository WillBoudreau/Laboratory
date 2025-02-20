using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorBehaviour : MonoBehaviour
{
    [Header("Reflector Settings")]
    [SerializeField] private float rotateSpeed = 1; // The speed at which the reflector rotates
    [SerializeField] private Vector3 axis;
    [SerializeField] private float rotateAngle; // The angle at which the reflector rotates
    public bool canRotate; // If the reflector can rotate
    private float currentRotation = 0; // Track the current rotation

    /// <summary>
    /// Rotate the reflector along the specified axis
    /// </summary>
    public void RotateReflector()
    {
        // Calculate the rotation for this frame
        float rotationThisFrame = rotateAngle * Time.deltaTime * rotateSpeed;
        transform.Rotate(axis, rotationThisFrame);
        currentRotation += rotationThisFrame;

        if (currentRotation >= rotateAngle)
        {
            canRotate = false;
            currentRotation = 0; // Reset for the next rotation
        }
    }

    public IEnumerator RotateReflectorCoroutine()
    {
        canRotate = true;
        while (canRotate)
        {
            RotateReflector();
            yield return null;
        }
    }
}
