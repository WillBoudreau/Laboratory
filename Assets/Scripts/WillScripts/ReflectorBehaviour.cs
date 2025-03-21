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
    public GameObject prism;
    void Start()
    {
        if(reflectorType == ReflectorType.stationary)
        {
            canRotate = false;
        }
        else if(reflectorType == ReflectorType.rotating)
        {
            canRotate = true;
        }
    }
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
            prism.transform.Rotate(axis, rotationThisFrame);
            currentRotation += rotationThisFrame;

            if (currentRotation >= rotateAngle)
            {
                float overshoot = currentRotation - rotateAngle;
                prism.transform.Rotate(axis, -overshoot);
                currentRotation = 0; // Reset for the next rotation
                canRotate = false;
            }   
        }
        else if(reflectorType == ReflectorType.rotating)
        {
            // Calculate the rotation for this frame
            float rotationThisFrame = rotateAngle * Time.deltaTime * rotateSpeed;
            prism.transform.Rotate(axis, rotationThisFrame);
            currentRotation += rotationThisFrame;

            if (currentRotation == rotateAngle)
            {
                canRotate = true;
                currentRotation = 0; // Reset for the next rotation
            }
        }
    }
    public IEnumerator RotateReflectorCoroutine()
    {
        //canRotate = true;
        if(reflectorType == ReflectorType.stationary)
        {
            canRotate = true;
        }
        while(canRotate)
        {
            RotateReflector();
            yield return null;
        }
    }
}
