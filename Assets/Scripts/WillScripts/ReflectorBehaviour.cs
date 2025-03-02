using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorBehaviour : MonoBehaviour
{
    [Header("Reflector Settings")]
    [SerializeField] private float rotateSpeed = 1; // The speed at which the reflector rotates
    [SerializeField] private Vector3 axis;
    [SerializeField] private float rotateAngle; // The angle at which the reflector rotates
    [SerializeField] private enum ReflectorType { stationary, rotating }; // The type of reflector
    [SerializeField] private ReflectorType reflectorType; // The type of reflector
    public bool canRotate; // If the reflector can rotate
    private float currentRotation = 0; // Track the current rotation

    void Update()
    {
        if(reflectorType == ReflectorType.rotating)
        {
            StartCoroutine(RotateReflectorCoroutine());
        }
    }
    /// <summary>
    /// Rotate the reflector along the specified axis
    /// </summary>
    public void RotateReflector()
    {
        if(reflectorType == ReflectorType.stationary)
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
        else if(reflectorType == ReflectorType.rotating)
        {
            // Calculate the rotation for this frame
            float rotationThisFrame = rotateAngle * Time.deltaTime * rotateSpeed;
            transform.Rotate(axis, rotationThisFrame);
            currentRotation += rotationThisFrame;

            if (currentRotation >= rotateAngle)
            {
                canRotate = true;
                currentRotation = 0; // Reset for the next rotation
            }
        }
    }
    public IEnumerator RotateReflectorCoroutine()
    {
        while (canRotate)
        {
            RotateReflector();
            yield return null;
        }
    }
}
