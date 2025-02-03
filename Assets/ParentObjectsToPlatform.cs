using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObjectsToPlatform : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            other.gameObject.transform.SetParent(this.gameObject.transform,true); 
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            other.gameObject.transform.SetParent(null,true);
        }
    }
}
