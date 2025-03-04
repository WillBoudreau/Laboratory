using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    [Header("Timeline Settings")]
    [SerializeField] private PlayableDirector timeline; // The timeline to play
    [SerializeField] private GameObject player; // The player object
    [SerializeField] private Singleton singleton; // The singleton object
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        singleton = FindObjectOfType<Singleton>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.CompareTag("Elevator"))
        {
            Debug.Log("Platform Triggered");
            other.transform.GetComponent<ElevatorBehaviour>().isAbleToParent = false;
            timeline.Play();
        }
    }
}
