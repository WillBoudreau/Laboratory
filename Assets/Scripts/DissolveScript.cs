using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveScript : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] skinnedMaterials;

    // Start is called before the first frame update
    void Start()
    {
        if (skinnedMesh != null)
            skinnedMaterials = skinnedMesh.materials;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DissolveCo());
        }
    }
    IEnumerator DissolveCo()
    {
        if(skinnedMaterials.Length > 0)
        {
            float counter = 0;
            while (skinnedMaterials[0].GetFloat("") < 1)
            {
                counter += dissolveRate;
                for(int i=0; i < skinnedMaterials.Length;)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
