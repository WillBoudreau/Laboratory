using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intractable : MonoBehaviour
{
    public SFXTrigger sFXTrigger;
    private Rigidbody rb;
    public MeshRenderer meshRenderer;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(rb != null && sFXTrigger != null)
        {
            if(rb.velocity.x != 0 && !sFXTrigger.source3D.isPlaying)
            {
                sFXTrigger.PlaySFX(1);
            }
            else if(rb.velocity.x == 0 && sFXTrigger.source3D.isPlaying)
            {
                sFXTrigger.source3D.Stop();
            }
        }
    }
}
