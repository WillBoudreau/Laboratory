using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class DisplayDialogue : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private List<string> dialogueText; // The dialogue text
    [SerializeField] private TextMeshProUGUI dialogueTextDisplay; // The dialogue text
    [SerializeField] private GameObject dialogueTextDisplayPanel; // The dialogue text graphic
    [SerializeField] private int currentDialogueIndex = 0; // The current dialogue index
    [SerializeField] private float timeBetweenText = 0.025f; // The time between each letter being displayed
    [SerializeField] private float timeAtEndOfText = 10f; // The time at the end of the text before it is cleared
    [SerializeField] private bool isDialogueActive = false; // Is the dialogue active
    [Header("Class calls")]
    public LocalizationComponent localizationComponent; // The localization component
    [SerializeField] private DialogueManager dialogueManager; // The dialogue manager

    private Coroutine currentCoroutine; // Track the active coroutine

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    /// <summary>
    /// Set the loading screen
    /// </summary>
    public void SetDialogue()
    {
        dialogueManager.GetAllDialogues();
        dialogueManager.CheckActiveDialogue();
        SetText();
        ChangeIndex();
    }

    /// <summary>
    /// Set the text that is displayed to the player
    /// </summary>
    void SetText()
    {
        DisplayInfo(dialogueTextDisplayPanel);
        currentCoroutine = StartCoroutine(TypeWriter(dialogueText[currentDialogueIndex], dialogueTextDisplayPanel, dialogueTextDisplay));
    }

    /// <summary>
    /// Display the text to the player
    /// </summary>
    /// <param name="textPanel"></param>
    void DisplayInfo(GameObject textPanel)
    {
        if (textPanel != null)
        {
            textPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Change the index of the dialogue text
    /// </summary>
    public void ChangeIndex()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex >= dialogueText.Count)
        {
            currentDialogueIndex = 0;
        }
    }

    /// <summary>
    /// Close the Dialogue text panel
    /// </summary>
    public void CloseDialogueTextPanel()
    {
        if (dialogueTextDisplayPanel != null)
        {
            dialogueTextDisplayPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Load the text in typewriter style
    /// </summary>
    /// <param name="text"></param>
    /// <param name="textPanel"></param>
    /// <param name="textDisplay"></param>
    IEnumerator TypeWriter(string text, GameObject textPanel, TextMeshProUGUI textDisplay)
    {
        textPanel.SetActive(true);
        textDisplay.text = string.Empty;
        foreach (char letter in text.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(timeBetweenText);
        }
        yield return new WaitForSeconds(timeAtEndOfText);
        textDisplay.text = string.Empty;
        textPanel.SetActive(false);
    }
}