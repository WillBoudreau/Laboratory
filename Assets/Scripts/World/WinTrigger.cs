using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;//The player object
    [SerializeField] private UIManager uIManager;//The UI manager object
    void Start()
    {
        //If the player is not set, find the player
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        //If the UI manager is not set, find the UI manager
        if(uIManager == null)
        {
            uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //If the object is the player
        if(other.gameObject == player)
        {
            uIManager.LoadUI("Win");
        }
    }

}
