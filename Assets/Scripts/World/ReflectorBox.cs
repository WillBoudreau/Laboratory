using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ReflectorBox : MonoBehaviour
{
    public GameObject core;

    // Update is called once per frame
    void Update()
    {
        core.transform.rotation = new Quaternion(0,0,0,1);
    }
}
