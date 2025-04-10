using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class MeshDissolveScript : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    public Collider col;
    public Rigidbody rb;
    private Material[] meshMaterials;
    public GameObject particles;

    void Start()
    {
        if (meshRenderer != null)
            meshMaterials = meshRenderer.materials;
        particles.SetActive(false);
    }

    // Call this to start dissolving
    public void StartDissolve()
    {
        FindObjectOfType<PlayerController>().interactionPosable = false;
        StartCoroutine(DissolveCo());
    }

    IEnumerator DissolveCo()
    {
        
        particles.SetActive(true);
        rb.useGravity = false;
        rb.velocity = rb.velocity.normalized;
        if (VFXGraph != null)
            VFXGraph.Play();
        col.enabled = false;
        if (meshMaterials.Length > 0)
        {
            float counter = 0;
            while (meshMaterials[0].GetFloat("_Dissolve_Amount") < 1)
            {
                counter += dissolveRate;
                foreach (var mat in meshMaterials)
                {
                    mat.SetFloat("_Dissolve_Amount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
            Destroy(this.gameObject);
        }
    }

    // Call this to instantly reset dissolve
    public void ResetDissolve()
    {
        float counter = 1;
        if (meshMaterials.Length > 0)
        {
            while (meshMaterials[0].GetFloat("_Dissolve_Amount") > 0)
            {
                counter -= dissolveRate;
                foreach (var mat in meshMaterials)
                {
                    mat.SetFloat("_Dissolve_Amount", counter);
                }
            }
        }
    }
}
