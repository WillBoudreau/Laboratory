using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;
    /// <summary>
    /// The instance of the Singleton
    /// </summary>
    public static Singleton Instance
    {
        //If the instance is null, find the instance
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Singleton>();
                if(instance == null)
                {
                    instance = new GameObject("Singleton").AddComponent<Singleton>();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        //If the instance is null, set the instance to this
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //If the instance is not this, destroy this
        else
        {
            Destroy(gameObject);
        }
    }
}
