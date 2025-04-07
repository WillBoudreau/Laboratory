using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCaller : MonoBehaviour
{
    public DissolveScript dissolveScript;

    // This could be called based on any condition you want
    void Start()
    {
        dissolveScript.StartDissolve();
    }
}