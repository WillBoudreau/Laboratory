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
            Debug.Log(other.gameObject.name + " is now a child of " + this.gameObject.name);
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Debug.Log(rb.velocity);
            // Accessing the physic material of the box
            Collider boxCollider = other.gameObject.GetComponent<Collider>();
            if (boxCollider != null)
            {
                PhysicMaterial physicMaterial = boxCollider.material;
                physicMaterial.dynamicFriction = 0f;
                physicMaterial.staticFriction = 0f;
                Debug.Log(physicMaterial.dynamicFriction);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Debug.Log(rb.velocity);
            Collider boxCollider = other.gameObject.GetComponent<Collider>();
            if (boxCollider != null)
            {
                PhysicMaterial physicMaterial = boxCollider.material;
                physicMaterial.dynamicFriction = 0f;
                physicMaterial.staticFriction = 0f;
                Debug.Log(physicMaterial.dynamicFriction);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            other.gameObject.transform.SetParent(null, true);
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Debug.Log(rb.velocity);
        }
    }
}
