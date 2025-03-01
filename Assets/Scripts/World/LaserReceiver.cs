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
    [SerializeField] private enum ReceiverType { Charged, Uncharged };
    [SerializeField] private ReceiverType receiverType;
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
            if(receiverType == ReceiverType.Charged)
            {
                DeactivateObjects();
            }
        }
    }
    /// <summary>
    /// Activate the Objects that link to the receiver
    /// </summary>
    void ActivateObjects()
    {
        foreach(GameObject obj in objectsToActivate)
        {
            switch(obj.tag)
            {
                case "Door":
                    obj.GetComponent<DoorBehaviour>().OpenThisDoor();
                    break;
                case "Platform":
                    obj.GetComponent<MovingPlatform>().canMove = true;
                    break;
                case "BoxDispenser":
                    obj.GetComponent<BoxSpawner>().SpawnBox();
                    break;
                case "Receiver":
                    Debug.Log("Deactivating Receiver");
                    obj.GetComponent<LaserEmitter>().deActivated = true;
                    break;
                case "Reflector":
                    obj.GetComponent<ReflectorBehaviour>().StartCoroutine("RotateReflectorCoroutine");
                    break;
            }
        }
        if(receiverType == ReceiverType.Charged)
        {
            isReceivingLaser = false;
        }
    }
    /// <summary>
    /// Deactivate the Objects that link to the receiver
    /// </summary>
    void DeactivateObjects()
    {
        foreach(GameObject obj in objectsToActivate)
        {
            if(obj.tag == "Door")
            {
                obj.GetComponent<DoorBehaviour>().CloseThisDoor();
            }
            else if(obj.tag == "Platform")
            {
                obj.GetComponent<MovingPlatform>().canMove = false;
            }
        }
    }
}
