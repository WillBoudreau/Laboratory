using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlatformTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<MovingPlatform>(out MovingPlatform platform))
        {
            if(platform.canMove)
            {
                platform.canMove = false;
            }
        }
    }
}
