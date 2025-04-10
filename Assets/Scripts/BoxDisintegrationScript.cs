using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class MeshDissolveScript : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] meshMaterials;

    void Start()
    {
        if (meshRenderer != null)
            meshMaterials = meshRenderer.materials;
    }

    // Call this to start dissolving
    public Coroutine StartDissolve()
    {
        return StartCoroutine(DissolveCo());
    }

    IEnumerator DissolveCo()
    {
        if (VFXGraph != null)
            VFXGraph.Play();

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
