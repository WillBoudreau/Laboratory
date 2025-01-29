using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [Header("Box Settings")]
    [SerializeField] private GameObject boxPrefab;//The box prefab
    [SerializeField] private GameObject[] spawnedBoxes;//The spawned boxes
    [SerializeField] private Transform spawnPoint;//The spawn point
    [SerializeField] private float spawnTime = 2f;//The time between spawns
    [SerializeField] private float spawnDelay = 1f;//The delay before the first spawn

    // Start is called before the first frame update
    void Start()
    {

    }
    /// <summary>
    /// Spawns a box at the spawn point after running a check to make sure not to spawn a box if one is already there
    /// </summary>
    void SpawnBox()
    {
        if(spawnedBoxes.Length > 0)
        {
            foreach(GameObject box in spawnedBoxes)
            {
                if(box == null)
                {
                    InstantiateBox();
                }
            }
        }
        else
        {
            InstantiateBox();
        }
    }
    /// <summary>
    /// Instantiates a box at the spawn point
    /// </summary>
    void InstantiateBox()
    {
        //Instantiate a box at the spawn point
        GameObject box = Instantiate(boxPrefab, spawnPoint.position, Quaternion.identity);
        //Add the box to the spawned boxes array
        spawnedBoxes = new GameObject[spawnedBoxes.Length + 1];
        //Set the last element in the array to the box
        spawnedBoxes[spawnedBoxes.Length - 1] = box;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            InvokeRepeating("SpawnBox", spawnDelay, spawnTime);
        }
    }
}
