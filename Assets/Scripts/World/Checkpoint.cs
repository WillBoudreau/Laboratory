using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool canBeActivated;
    public bool leftLevelFlow;
    public bool isFacingLeft;
    // Start is called before the first frame update
    void Start()
    {
        canBeActivated = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out PlayerController player) && canBeActivated)
        {
            player.CheckpointHit(this);
        }
    }

}
