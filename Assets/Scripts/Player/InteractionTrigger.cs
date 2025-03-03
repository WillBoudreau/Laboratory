using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public PlayerController player;
    public Material outline;
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
        if(other.gameObject.TryGetComponent<Intractable>(out Intractable interaction))
        {
            player.interactionPosable = true;
            List<Material> materials = new List<Material>();
            materials.Add(interaction.meshRenderer.material);
            materials.Add(outline);
            interaction.meshRenderer.SetMaterials(materials);
            if(interaction.gameObject.tag == "Box")
            {
                player.interactionTarget = interaction.gameObject;
            }
        }
    }

    /// <summary>
    /// Event called when object exits trigger. 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent<Intractable>(out Intractable interaction))
        {
            List<Material> materials = new List<Material>();
            materials.Add(interaction.meshRenderer.material);
            interaction.meshRenderer.SetMaterials(materials);
            player.interactionPosable = false;
            player.interactionTarget = null;
        }
    }
}
