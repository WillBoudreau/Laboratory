using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    [Header("Laser Receiver Settings")]
    public MeshRenderer marker;
    public bool isReceivingLaser;
    public Material basic;
    public Material hit;
    [SerializeField] private GameObject[] objectsToActivate;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isReceivingLaser)
        {
            marker.sharedMaterial = hit;
            ActivateObjects();
        }
        else
        {
            marker.sharedMaterial = basic;
        }
    }
    /// <summary>
    /// Activate the Objects that link to the receiver
    /// </summary>
    void ActivateObjects()
    {
        foreach(GameObject obj in objectsToActivate)
        {
            if(obj.tag == "Door")
            {
                obj.GetComponent<DoorBehaviour>().OpenThisDoor();
            }
            else if(obj.tag == "Platform")
            {
                obj.GetComponent<MovingPlatform>().canMove = true;
            }
            else if(obj.tag == "BoxDispenser")
            {
                obj.GetComponent<BoxSpawner>().SpawnBox();
            }
        }
    }
}
