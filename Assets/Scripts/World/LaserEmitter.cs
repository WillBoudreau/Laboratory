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
    [SerializeField] private enum LaserType {Normal,Damaged};//The type of laser
    [SerializeField] private LaserType laserType;//The type of laser
    [Header("Damaged Laser Settings")]
    [SerializeField] private float timeBetweenBurst;//The time between each burst
    [SerializeField] private float coolDownTimer;//The cooldown timer
    [SerializeField] private float timeBetweenShots;//The time between each shots, acts as a burst reset
    public bool isButtonActivated;//If the button is activated
    public bool deActivated;//If the laser is deactivated
    private Ray ray;
    private RaycastHit raycastHit;
    private Vector3 laserDirection;
    public LaserReceiver laserReceiver;
    public AudioSource sFXSource;
    public bool isArray;
    public GameObject sourceParticle;
    public GameObject particlePrefab;
    public GameObject[] collisionParticles;

    void Awake()
    {
        SetUpParticles();
        sourceParticle.SetActive(false);
        lineRenderer = GetComponent<LineRenderer>();
        if(isArray)
        {
            sFXSource.volume = 0.075f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsLaserActivated() == true)
        {
            //If the laser is Normal, continue to fire the laser
            if(LaserType.Normal == laserType && !isButtonActivated)
            {
                FireLaser();
            }
            // if the laser is damaged, fire the laser in bursts
            else if(LaserType.Damaged == laserType)
            {
                LaserBurstFire();
            }
        }
        else
        {
            DisableLaser();
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
        collisionParticles[index].transform.position = hit.point;
    }

    void DisableParticles()
    {
        foreach (var particle in collisionParticles)
        {
            particle.SetActive(false);
        }
    }

    /// <summary>
    /// Fire the laser in bursts
    /// <summary>
    void LaserBurstFire()
    {
        //If the time between bursts is less than or equal to 0, stop firing the laser
        if(timeBetweenBurst <= 0)
        {
            DisableLaser();
            coolDownTimer -= Time.deltaTime;

            //If the cooldown timer is less than or equal to 0, reset the cooldown timer
            if(coolDownTimer <= 0)
            {
                //Reset the cooldown timer and the time between bursts
                coolDownTimer = timeBetweenShots;
                timeBetweenBurst = timeBetweenShots;
            }
        }
        else
        {
            //Decrease the time between bursts
            timeBetweenBurst -= Time.deltaTime;
            lineRenderer.positionCount = 1;
            FireLaser();
        }
    }
    /// <summary>
    /// Disable the laser
    /// </summary>
    public void DisableLaser()
    {
        lineRenderer.positionCount = 0;
        DisableParticles();
        sourceParticle.SetActive(false);
    }
    /// <summary>
    /// Determine whether the laser is activated or deactivated
    /// </summary>
    public bool IsLaserActivated()
    {
        if(!deActivated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Switch laser type
    /// </summary>
    public void SwitchLaserType()
    {
        if(laserType == LaserType.Normal)
        {
            laserType = LaserType.Damaged;
        }
        else
        {
            laserType = LaserType.Normal;
        }
    }

    /// <summary>
    /// Fire the laser
    /// </summary>
    public void FireLaser()
    {
        sourceParticle.SetActive(true);
        ray = new Ray(transform.position,transform.right);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0,transform.position);
        float remainingLength = laserLength;

        for(int i = 0; i< maxReflections; i++)
        {
            if(Physics.Raycast(ray.origin,ray.direction, out raycastHit, remainingLength))
            {
                laserReceiver = raycastHit.collider.gameObject.GetComponent<LaserReceiver>();
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount-1,raycastHit.point);
                remainingLength -= Vector3.Distance(ray.origin,raycastHit.point);
                SetParticlePos(i,raycastHit);
                if(raycastHit.collider.tag == "Reflector")
                {
                    ray = new Ray(raycastHit.point, Vector3.Reflect(ray.direction, raycastHit.normal));
                }
                if(raycastHit.collider.tag == "ReflectorBox")
                {
                    if(raycastHit.collider.gameObject.TryGetComponent<ReflectorBox>(out ReflectorBox box))
                    {
                        if(isArray)
                        {
                            Destroy(raycastHit.collider.gameObject.transform.parent.gameObject);
                        }
                        else
                        {
                            switch(box.direction)
                            {
                                case ReflectorBox.Direction.right:
                                    ray = new Ray(raycastHit.transform.position, Vector3.right);
                                    break;
                                case ReflectorBox.Direction.left:
                                    ray = new Ray(raycastHit.transform.position, Vector3.left);
                                    break;
                                case ReflectorBox.Direction.up:
                                    ray = new Ray(raycastHit.transform.position, Vector3.up);
                                    break;
                                case ReflectorBox.Direction.down:
                                    ray = new Ray(raycastHit.transform.position, Vector3.down);
                                    break;
                                default:
                                    ray = new Ray(raycastHit.transform.position, Vector3.right);
                                    break;
                            }
                        }
                    }
                }
                else if(raycastHit.collider.tag == "Receiver")
                {
                    laserReceiver.isReceivingLaser = true;
                    break;
                }
                else if(raycastHit.collider.tag == "Receiver" && laserReceiver.isReceivingLaser)
                {
                    laserReceiver.isReceivingLaser = false;
                    break;
                }
                else if(raycastHit.collider.tag != "Reflector" || raycastHit.collider.tag == "ReflectorBox")
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
}
