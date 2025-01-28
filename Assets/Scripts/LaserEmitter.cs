using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour
{
    [SerializeField]
    private int maxReflections;
    [SerializeField]
    private float laserLength;
    [SerializeField]
    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit raycastHit;
    private Vector3 laserDirection;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(transform.position,transform.right);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0,transform.position);
        float remainingLength = laserLength;

        for(int i = 0; i< maxReflections; i++)
        {
            if(Physics.Raycast(ray.origin,ray.direction, out raycastHit, remainingLength))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount-1,raycastHit.point);
                remainingLength -= Vector3.Distance(ray.origin,raycastHit.point);
                ray = new Ray(raycastHit.point, Vector3.Reflect(ray.direction, raycastHit.normal));
                if(raycastHit.collider.tag != "Reflector")
                    break;
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount-1,ray.origin + ray.direction * remainingLength);
            }
        }
    }
}
