using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenBehavior : MonoBehaviour
{
    [Header("Loading Screen Settings")]
    [SerializeField] private List<GameObject> loadingScreenInfoGraphics;//The loading screen info graphics
    [SerializeField] private List<string> loadingScreenInfoText;//The loading screen info text
    [SerializeField] private TextMeshProUGUI loadingScreenInfoTextDisplay;//The loading screen info text display
    [SerializeField] private GameObject loadingScreenInfoGraphicDisplay;//The loading screen info graphic display
    [SerializeField] private int currentLoadingScreenInfoIndex = 0;//The current loading screen info index
    /// <summary>
    /// Set the loading screen
    /// </summary>
    public void SetLoadingScreen()
    {
        SetText();
        ChangeIndex();
    }
    /// <summary>
    /// Display the next loading screen info
    /// </summary>
    void DisplayInfo(GameObject infoGraphic, string infoText)
    {
        //Set the loading screen info graphic
        foreach(GameObject graphic in loadingScreenInfoGraphics)
        {
            graphic.SetActive(false);
        }
        infoGraphic.SetActive(true);
        loadingScreenInfoGraphicDisplay = infoGraphic;
        //Set the loading screen info text
        loadingScreenInfoTextDisplay.text = infoText;
    }
    /// <summary>
    /// Set the text based on the amount of hints in the list
    /// </summary>
    void SetText()
    {
        //Set the loading screen info text
        loadingScreenInfoTextDisplay.text = loadingScreenInfoText[currentLoadingScreenInfoIndex];
        DisplayInfo(loadingScreenInfoGraphics[currentLoadingScreenInfoIndex], loadingScreenInfoText[currentLoadingScreenInfoIndex]);
    }
    /// <summary>
    /// Change the index of the loading screen info
    /// </summary>
    void ChangeIndex()
    {
        Debug.Log("Changing index");
        //If the current index is less than the amount of hints in the list
        if(currentLoadingScreenInfoIndex < loadingScreenInfoText.Count - 1)
        {
            //Increase the index
            currentLoadingScreenInfoIndex++;
        }
        else
        {
            //Reset the index
            currentLoadingScreenInfoIndex = 0;
        }
    }
}
