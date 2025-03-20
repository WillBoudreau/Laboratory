using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [Header("Box Settings")]
    [SerializeField] private GameObject boxPrefab; // The box prefab
    [SerializeField] private GameObject spawnedBox; // The spawned box
    [SerializeField] private Transform spawnPoint; // The spawn point
    [SerializeField] private AudioClip spawnSound; // The sound to play when the box is spawned
    [SerializeField] private AudioSource audioSource; // The audio source component

    /// <summary>
    /// Spawns a box at the spawn point if there is no box already spawned
    /// </summary>
    public void SpawnBox()
    {
        if (spawnedBox == null)
        {
            PlaySpawnSound();
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

    /// <summary>
    /// Plays the spawn sound
    /// </summary>
    void PlaySpawnSound()
    {
        Debug.Log("Playing box spawn sound");
        // Play the spawn sound
        audioSource.PlayOneShot(spawnSound);
    }
}
