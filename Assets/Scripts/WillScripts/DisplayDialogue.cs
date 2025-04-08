using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class DisplayDialogue : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private List<string> dialogueText; // The dialogue text
    public TextMeshProUGUI dialogueTextDisplay; // The dialogue text
    public GameObject dialogueTextDisplayPanel; // The dialogue text graphic
    [SerializeField] private int currentDialogueIndex = 0; // The current dialogue index
    [SerializeField] private float timeBetweenText = 0.025f; // The time between each letter being displayed
    public bool isDialogueActive = false; // Is the dialogue active
    [Header("Class calls")]
    public LocalizationComponent localizationComponent; // The localization component
    [SerializeField] private DialogueManager dialogueManager; // The dialogue manager
    [SerializeField] private VoiceLineManager voiceLineManager; // The voiceline manager
    [SerializeField] private VoiceLineTrigger voiceLineTrigger;// The voiceline tigger

    private Coroutine currentCoroutine; // Track the active coroutine

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        voiceLineManager = FindObjectOfType<VoiceLineManager>();
    }
    /// <summary>
    /// Set the loading screen
    /// </summary>
    public void SetDialogue(GameObject activeDialogue)
    {
        SetTextToLocalizationComponent();
        currentCoroutine = StartCoroutine(TypeWriter(localizationComponent.localizedSTR.GetLocalizedString(), dialogueTextDisplayPanel, dialogueTextDisplay));  
        dialogueManager.GetAllDialogues();
        dialogueManager.CheckActiveDialogue(activeDialogue);
    }

    /// <summary>
    /// Set the text that is displayed to the player
    /// </summary>
    void SetText()
    {
        DisplayInfo(dialogueTextDisplayPanel);
        currentCoroutine = StartCoroutine(TypeWriter(dialogueText[currentDialogueIndex], dialogueTextDisplayPanel, dialogueTextDisplay));
    }
    ///<summary>
    /// Set the text to the localization component
    /// </summary>
    public void SetTextToLocalizationComponent()
    {
        if (localizationComponent != null)
        {
            localizationComponent.SetupLocalizationString();
            localizationComponent.UpdateText(dialogueText[currentDialogueIndex]);
        }
    }
    /// <summary>
    /// Reset the dialogue to match the locale 
    /// </summary>
    public void ResetDialogue(GameObject activeDialogue)
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
            dialogueManager.GetAllDialogues();
            dialogueManager.CheckActiveDialogue(activeDialogue);
        }
        dialogueTextDisplay.text = string.Empty;
        string localizedText = localizationComponent.localizedSTR.GetLocalizedString();
        currentCoroutine = StartCoroutine(TypeWriter(localizedText, dialogueTextDisplayPanel, dialogueTextDisplay));
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
        Debug.Log("Clearing text");
        yield return new WaitForSeconds(voiceLineManager.voiceLines[voiceLineTrigger.voiceLineIndex].length);
        textPanel.SetActive(false);
        textDisplay.text = string.Empty;
    }
}