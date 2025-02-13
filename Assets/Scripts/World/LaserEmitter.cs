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
    public LaserReceiver laserReceiver;
    public AudioSource sFXSource;
    public bool isArray;
    public GameObject particlePrefab;
    public GameObject[] collisionParticles;

    void Awake()
    {
        SetUpParticles();
        lineRenderer = GetComponent<LineRenderer>();
        if(isArray)
        {
            sFXSource.volume = 0.075f;
        }
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
                SetParticlePos(i,raycastHit);
                ray = new Ray(raycastHit.point, Vector3.Reflect(ray.direction, raycastHit.normal));
                if(raycastHit.collider.tag == "Receiver")
                {
                    laserReceiver = raycastHit.collider.gameObject.GetComponent<LaserReceiver>();
                    laserReceiver.isReceivingLaser = true;
                    break;
                }
                else if(raycastHit.collider.tag != "Reflector")
                {
                    if(raycastHit.collider.tag == "Player")
                    {
                        if(!raycastHit.collider.gameObject.GetComponent<PlayerController>().isHurt)
                        {
                            raycastHit.collider.gameObject.GetComponent<PlayerController>().TakeDamage();
                            raycastHit.collider.gameObject.GetComponent<PlayerController>().TakeDamage();
                        }   
                    }
                    if(raycastHit.collider.tag == "Box")
                    {
                        Destroy(raycastHit.collider.gameObject);
                    }
                    if(laserReceiver != null)
                    {
                        laserReceiver.isReceivingLaser = false;
                        laserReceiver = null;
                    }
                    break;
                }
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount-1,ray.origin + ray.direction * remainingLength);
            }
        }
    }

    void SetUpParticles()
    {
        collisionParticles = new GameObject[maxReflections];
        for(int i = 0; i < maxReflections; i++)
        {
            collisionParticles[i] = Instantiate(particlePrefab,this.transform);
            collisionParticles[i].SetActive(false);
        }
    }

    void SetParticlePos(int index, RaycastHit hit)
    {
        collisionParticles[index].SetActive(true);
        collisionParticles[index].transform.position = hit.collider.gameObject.transform.position;
    }
}
