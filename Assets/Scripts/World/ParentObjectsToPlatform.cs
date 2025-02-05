using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObjectsToPlatform : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            other.gameObject.transform.SetParent(this.gameObject.transform, true);
            //Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
        }
    }
    // void OnTriggerStay(Collider other)
    // {
    //     if(other.gameObject.tag == "Box")
    //     {
    //         Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
    //         rb.velocity = Vector3.zero;
    //         rb.angularVelocity = Vector3.zero;
    //     }
    // }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            other.gameObject.transform.SetParent(null, true);
        }
    }
}
