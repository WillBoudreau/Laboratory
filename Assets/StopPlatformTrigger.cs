using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlatformTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.TryGetComponent<MovingPlatform>(out MovingPlatform platform))
        {
            Debug.Log("Stop Platform");
            if(platform.canMove)
            {
                platform.canMove = false;
            }
        }
    }
}
