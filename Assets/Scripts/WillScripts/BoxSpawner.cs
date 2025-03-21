using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [Header("Box Settings")]
    [SerializeField] private GameObject boxPrefab; // The box prefab
    [SerializeField] private GameObject spawnedBox; // The spawned box
    [SerializeField] private Transform spawnPoint; // The spawn point for the box
    [Header("Audio Settings")]
    [SerializeField] private SFXManager sFXManager; // The SFX manager
    void Awake()
    {
        sFXManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
    }
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
        // Play the box spawn sound
        sFXManager.Player2DSFX(sFXManager.boxDispenserSFX, false);
        // Instantiate a box at the spawn point
        spawnedBox = Instantiate(boxPrefab, spawnPoint.position, boxPrefab.transform.rotation);
    }
}
