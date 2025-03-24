using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehavior : MonoBehaviour
{
    [Header("Sound settings")]
    [SerializeField] private SFXManager sFXManager; // The SFX manager
    bool isOnPlatform = false;
    void Awake()
    {
        sFXManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Receiver")
        {
            Destroy(this.gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            if(isOnPlatform == false)
            {
                sFXManager.Player2DSFX(sFXManager.boxLandSFX, false);
                isOnPlatform = true;
            }
        }
    }
}
