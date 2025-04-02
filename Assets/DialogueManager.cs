using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private List<GameObject> dialogues; // The dialogue

    /// <summary>
    /// Find all of the dialogue objects in the scene
    /// </summary>
    public void GetAllDialogues()
    {
        dialogues = new List<GameObject>(GameObject.FindGameObjectsWithTag("Dialogue"));
    }
    /// <summary>
    /// Check to make sure that only one dialogue is active at a time
    /// </summary>
    public void CheckActiveDialogue()
    {
        foreach (GameObject dialogue in dialogues)
        {
            if (dialogue.activeSelf)
            {
                dialogue.SetActive(false);
            }
        }
    }
}
