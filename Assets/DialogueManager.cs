using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public List<GameObject> dialogues = new List<GameObject>(); // The dialogue objects in the scene
    [SerializeField] private int maxActiveDialogues; // The currently active dialogue object
    [SerializeField] private LevelManager levelManager; // The level manager
    [SerializeField] private LoadingScreenBehavior loadingScreenBehavior; // The loading screen behavior

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }
    /// <summary>
    /// Find all of the dialogue objects in the scene
    /// </summary>
    public void GetAllDialogues()
    {
        dialogues = new List<GameObject>(GameObject.FindGameObjectsWithTag("Dialogue"));
        Debug.Log("Found " + dialogues.Count + " dialogues in the scene.");
    }
    /// <summary>
    /// Check to make sure that only one dialogue is active at a time
    /// </summary>
    public void CheckActiveDialogue(GameObject activeDialogue)
    {
        Debug.Log("Checking active dialogues...");
        Debug.Log("Active dialogue: " + activeDialogue.name);
        //GetAllDialogues();
        foreach (GameObject dialogue in dialogues)
        {
            if(!dialogues.Contains(dialogue))
            {
                dialogue.SetActive(false);
            }
            else if (dialogue != activeDialogue)
            {
                dialogue.SetActive(false);
            }
            else if (dialogue == activeDialogue)
            {
                dialogue.SetActive(true);
            }
        }
        GetAllDialogues();
    }
    /// <summary>
    /// Reinitialize the dialogue manager to ensure that the correct dialogue is active before the player switches the locale
    /// </summary>
    public void ReinitializeDialogueManager()
    {
        foreach (GameObject dialogue in dialogues)
        {
            if (dialogue.activeSelf)
            {
                CheckActiveDialogue(dialogue);
            }
        }
    }
    /// <summary>
    /// Make sure that the entire dialogue object including children are active
    /// </summary>
    public void DisplayInfo(GameObject dialogue)
    {
        GetAllDialogues();
        gameObject.GetComponent<DisplayDialogue>().SetDialogue(dialogue);
    }
}
