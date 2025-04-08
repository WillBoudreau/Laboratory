using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ButtonV2 : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private GameObject[] objectsToControl; // The object the button controls
    [SerializeField] private bool canPlayerInteract = true;
    [SerializeField] private float upwardVelocity = 1;

    [Header("Audio Settings")]
    [SerializeField] private SFXManager sfxManager;//The SFX manager

    private Rigidbody rb;
    [HideInInspector] public bool isActivated;
    private int numberOfTouchingObjects;

    void Awake()
    {
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();

        numberOfTouchingObjects = 0;
        isActivated = false;
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y > 0.165f)
        {
            transform.localPosition = new Vector3(0, 0.165f, 0);
        }

        if (transform.localPosition.y < -0.03f)
        {
            transform.localPosition = new Vector3(0, -0.03f, 0);
        }

        if (transform.localPosition.y <= -0.01f && !isActivated)
        {
            ButtonActivated();
        }

        if (transform.localPosition.y > 0.05f && isActivated)
        {
            ButtonDeactivated();
        }

        if (numberOfTouchingObjects < 1)
            rb.velocity = new Vector3(0, upwardVelocity, 0);

    }

    private void ButtonActivated()
    {
        sfxManager.Player2DSFX(sfxManager.buttonSFX[1], false);

        //For each object in the array
        foreach (GameObject obj in objectsToControl)
        {
            switch (obj.tag)
            {
                //If the object is a door, open it
                case "Door":
                    obj.GetComponent<DoorBehaviour>().OpenThisDoor();
                    break;

                //If the object is a platform, allow it to move
                case "Platform":
                    obj.GetComponent<MovingPlatform>().SetMovementStatus(true);
                    break;

                case "Receiver":
                    obj.GetComponent<LaserEmitter>().FireLaser();
                    break;

                case "BoxDispenser":
                    obj.GetComponent<BoxSpawner>().SpawnBox();
                    break;

                case "Reflector":
                    obj.GetComponent<ReflectorBehaviour>().canRotate = false;
                    break;

                default:
                    Debug.LogError("Object under objectsToControl not tagged correctly or missing behaviour");
                    break;
            }
        }

        isActivated = true;
    }


    private void ButtonDeactivated()
    {
        sfxManager.Player2DSFX(sfxManager.buttonSFX[2], false);

        //For each object in the array
        foreach (GameObject obj in objectsToControl)
        {
            switch (obj.tag)
            {
                //If the object is a door, close it
                case "Door":
                    obj.GetComponent<DoorBehaviour>().CloseThisDoor();
                    break;

                //If the object is a platform, stop it from moving
                case "Platform":
                    obj.GetComponent<MovingPlatform>().SetMovementStatus(false);
                    break;

                case "Receiver":
                    obj.GetComponent<LaserEmitter>().DisableLaser();
                    break;

                case "BoxDispenser":
                    break;

                case "Reflector":
                    obj.GetComponent<ReflectorBehaviour>().canRotate = true;
                    obj.GetComponent<ReflectorBehaviour>().StartCoroutine("RotateReflectorCoroutine");
                    break;


                default:
                    Debug.LogError("Object under objectsToControl not tagged correctly or missing behaviour");
                    break;
            }
        }

        isActivated = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && canPlayerInteract)
        {
            numberOfTouchingObjects++;
        }

        if (collision.transform.CompareTag("Box"))
        {
            numberOfTouchingObjects++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && canPlayerInteract)
        {
            numberOfTouchingObjects--;
        }

        if (collision.transform.CompareTag("Box"))
        {

            numberOfTouchingObjects--;
        }
    }
}
