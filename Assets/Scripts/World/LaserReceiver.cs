using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    public MeshRenderer marker;
    public bool isReceivingLaser;
    public Material basic;
    public Material hit;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isReceivingLaser)
        {
            marker.sharedMaterial = hit; 
        }
        else
        {
            marker.sharedMaterial = basic;
        }
    }
}
