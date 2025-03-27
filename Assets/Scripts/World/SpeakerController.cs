using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerController : MonoBehaviour
{

    public Animator animator;
    public bool isPlaying;
    public GameObject particle;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isPlaying",isPlaying);
        particle.SetActive(isPlaying);
    }
}
