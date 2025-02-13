using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    /// <summary>
    /// Event called when object is inside trigger for more than 1 frame. 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<Intractable>(out Intractable pushable))
        {
            player.interactionPosable = true;
            if(pushable.gameObject.tag == "Box")
            {
                player.interactionTarget = pushable.gameObject;
            }
        }
    }

    /// <summary>
    /// Event called when object exits trigger. 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent<Intractable>(out Intractable pushable))
        {
            player.interactionPosable = false;
            player.interactionTarget = null;
        }
    }
}
