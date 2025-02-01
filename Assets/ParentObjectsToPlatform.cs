using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObjectsToPlatform : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            Vector3 originalScale = other.gameObject.transform.localScale;
            other.gameObject.transform.SetParent(this.gameObject.transform);
            other.gameObject.transform.localScale = originalScale;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            Vector3 originalScale = other.gameObject.transform.localScale;
            other.gameObject.transform.SetParent(null);
            other.gameObject.transform.localScale = originalScale;
        }
    }
}
