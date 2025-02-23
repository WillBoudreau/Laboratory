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
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.angularDrag = 0f;
            Debug.Log(rb.velocity);
            // Accessing the physic material of the box
            Collider boxCollider = other.gameObject.GetComponent<Collider>();
            if (boxCollider != null)
            {
                PhysicMaterial physicMaterial = boxCollider.material;
                physicMaterial.dynamicFriction = 0f;
                physicMaterial.staticFriction = 0f;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.angularDrag = 0f;
            Collider boxCollider = other.gameObject.GetComponent<Collider>();
            if (boxCollider != null)
            {
                PhysicMaterial physicMaterial = boxCollider.material;
                physicMaterial.dynamicFriction = 0f;
                physicMaterial.staticFriction = 0f;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            other.gameObject.transform.SetParent(null, true);
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        }
    }
}
