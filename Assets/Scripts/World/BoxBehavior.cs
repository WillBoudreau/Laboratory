using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehavior : MonoBehaviour
{
    [Header("Sound settings")]
    [SerializeField] private SFXManager sFXManager; // The SFX manager
    [SerializeField] private Rigidbody rb; // The rigidbody of the box
    [SerializeField] private float fallThreshold = -0.5f; // The threshold for falling (negative for downward velocity)
    [SerializeField] private bool hasFallen = false;

    void Awake()
    {
        if (sFXManager == null)
        {
            sFXManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the box is falling
        if (rb.velocity.y < fallThreshold) // If the box is falling downward
        {
            hasFallen = true;
        }
        if (collision.gameObject.tag == "Platform")
        {
            // Check if the box has fallen and the impact velocity is significant
            if (hasFallen)
            {
                sFXManager.Player2DSFX(sFXManager.boxLandSFX, false);
                hasFallen = false;
            }
        }
    }
}
