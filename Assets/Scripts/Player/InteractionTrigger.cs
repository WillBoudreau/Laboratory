using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public PlayerController player;
    public Material outline;
    private List<Material> materials;
    public List<Material> baseMats;
    public List<GameObject> targets;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        targets = new List<GameObject>();
        baseMats = new List<Material>();
    }

    /// <summary>
    /// Event called when object is inside trigger for more than 1 frame. 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<Intractable>(out Intractable interaction))
        {
            targets.Add(interaction.gameObject);
            player.interactionPosable = true;
            for(int i = 0; i < targets.Count; i++)
            {
                materials = new List<Material>();
                baseMats.Add(targets[i].GetComponent<Intractable>().meshRenderer.materials[0]);
                materials.Add(targets[i].GetComponent<Intractable>().meshRenderer.materials[0]);
                materials.Add(outline);
                interaction.meshRenderer.SetMaterials(materials);
            }
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
            for(int i = 0; i < targets.Count; i++)
            {
                if(interaction.gameObject == targets[i])
                {
                    materials = new List<Material>();
                    materials.Add(baseMats[i]);
                    targets[i].GetComponent<Intractable>().meshRenderer.SetMaterials(materials);
                    baseMats.RemoveAt(i);
                    targets.RemoveAt(i);
                }
            }
            player.interactionPosable = false;
            player.interactionTarget = null;
        }
    }
}
