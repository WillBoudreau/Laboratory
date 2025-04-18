using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoxTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Destroy(other.gameObject);
        }
    }
}
