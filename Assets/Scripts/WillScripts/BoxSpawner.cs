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
    [SerializeField] private VoiceLineManager voiceLineManager;
    private int boxSpawnCount;
    void Awake()
    {
        voiceLineManager = FindObjectOfType<VoiceLineManager>();
        sFXManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        boxSpawnCount = 0;
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
        Debug.Log("Box Dispenser is spawning a box");
        // Instantiate a box at the spawn point
        spawnedBox = Instantiate(boxPrefab, spawnPoint.position, boxPrefab.transform.rotation);
        boxSpawnCount += 1;
        if(boxSpawnCount % 3 == 0) //Every 3rd box will trigger the voice line. 
        {
            voiceLineManager.PlayVoiceLine(voiceLineManager.voiceLines[7]);
        }
    }
}
