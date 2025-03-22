using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    [Header("Laser Receiver Settings")]
    public bool isReceivingLaser;
    [SerializeField] private enum ReceiverType { Charged, Uncharged };
    [SerializeField] private ReceiverType receiverType;
    [SerializeField] private GameObject[] objectsToActivate;
    [SerializeField] private float activationTime = 0.5f;
    [Header("Debug Controls")]
    [SerializeField] private bool debugMode;
    [SerializeField] private bool debugActivate;
    [SerializeField] private bool debugDeactivate;

    // Update is called once per frame
    void Update()
    {
        if(isReceivingLaser)
        {
            CountDownActivationTime(activationTime);
        }
        else
        {
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
    void OnDrawGizmos()
    {
        if(debugMode)
        {
            // Draw lines from the reciever to the objects it controls
            Gizmos.color = Color.red;
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                {
                    Gizmos.DrawLine(transform.position, obj.transform.position);
                }
            }
            if(debugActivate)
            {
                isReceivingLaser = true;
            }
            if(debugDeactivate)
            {
                isReceivingLaser = false;
            }
        }
    }
    /// <summary>
    /// Count down the activation time until the receiver activates
    /// </summary>
    void CountDownActivationTime(float time)
    { 
        activationTime = time; 
        if(time > 0)
        {
            activationTime -= Time.deltaTime;
        }
        else if(time <= 0)
        {
            time = activationTime;
            ActivateObjects();
        }
    }

}
