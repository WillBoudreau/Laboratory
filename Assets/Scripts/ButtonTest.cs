using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private GameObject[] objectsToControl; // The object the button controls
    [SerializeField] private float upwardVelocity = 1;

    private Rigidbody rb;
    [HideInInspector] public bool isActivated;

    // Start is called before the first frame update
    void Start()
    {
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

        if (transform.localPosition.y < 0.01f)
        {
            transform.localPosition = new Vector3(0, 0.01f, 0);
        }

        if (transform.localPosition.y <= 0.015f && !isActivated)
        {
            ButtonActivated();
        }

        if (transform.localPosition.y > 0.015f && isActivated)
        {
            ButtonDeactivated();
        }

        rb.velocity = new Vector3(0, upwardVelocity, 0);

    }

    private void ButtonActivated()
    {
        //For each object in the array
        foreach (GameObject obj in objectsToControl)
        {
            //If the object is a door, open it
            if (obj.tag == "Door")
            {
                obj.GetComponent<DoorBehaviour>().OpenThisDoor();
            }
            //If the object is a platform, allow it to move
            else if (obj.tag == "Platform")
            {
                obj.GetComponent<MovingPlatform>().canMove = true;
            }
        }

        isActivated = true;
    }

    private void ButtonDeactivated()
    {
        //For each object in the array
        foreach (GameObject obj in objectsToControl)
        {
            //If the object is a door, close it
            if (obj.tag == "Door")
            {
                obj.GetComponent<DoorBehaviour>().CloseThisDoor();
            }
            //If the object is a platform, stop it from moving
            else if (obj.tag == "Platform")
            {
                obj.GetComponent<MovingPlatform>().canMove = false;
            }
        }

        isActivated = false;
    }
}
