using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [Header("Box Settings")]
    [SerializeField] private GameObject boxPrefab; // The box prefab
    [SerializeField] private GameObject spawnedBox; // The spawned box
    [SerializeField] private Transform spawnPoint; // The spawn point
    [SerializeField] private float spawnTime = 2f; // The time between spawns
    [SerializeField] private float spawnDelay = 1f; // The delay before the first spawn

    /// <summary>
    /// Spawns a box at the spawn point if there is no box already spawned
    /// </summary>
    public void SpawnBox()
    {
        if (spawnedBox == null)
        {
            InstantiateBox();
        }
    }

    /// <summary>
    /// Instantiates a box at the spawn point
    /// </summary>
    void InstantiateBox()
    {
        // Instantiate a box at the spawn point
        spawnedBox = Instantiate(boxPrefab, spawnPoint.position, boxPrefab.transform.rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InvokeRepeating("SpawnBox", spawnDelay, spawnTime);
        }
    }
}
