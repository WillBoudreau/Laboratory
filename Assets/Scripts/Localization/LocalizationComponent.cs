using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using TMPro;


public class LocalizationComponent : MonoBehaviour
{
  [Header("Localization Settings")]
  [SerializeField] private TextMeshProUGUI uIText;// The text that will change based on the selected locale.
  public string localizationKey;// The key used to identify the text in the localization table.
  public string localizationTableName;// The name of the localization table.
  [SerializeField] private float fontSize = 24;// The font size of the text.

  private LocalizedString localizedSTR = new LocalizedString();// The localized string object.

  void Start()
  {
    // Register the AdjustText method to be called when the selected locale changes.
    LocalizationSettings.SelectedLocaleChanged += AdjustText;
    SetupLocalizationString();
  }
  /// <summary>
  /// Updates the text component with the localized string.
  ///</summary>
  /// <param name="localizedString"></param>
  void UpdateText(string localizedString)
  {
    if(uIText != null)
    {
      uIText.text = localizedString;
    }
  }
  /// <summary>
  /// Set up the localization string
  /// <summary>
  public void SetupLocalizationString()
  {
    // Get the localized string based on the selected locale and update the text component.
    //localizedSTR.StringChanged -= UpdateText;
    localizedSTR.StringChanged -= UpdateText;
    localizedSTR = new LocalizedString { TableReference = localizationTableName, TableEntryReference = localizationKey };
    localizedSTR.StringChanged += UpdateText;
    localizedSTR.RefreshString();
  }

  /// <summary>
  /// Sets the language for the localization system.
  /// This method changes the selected locale based on the provided language string.
  /// </summary>
  /// <param name="newLanguage"></param>
  public void SetLanguage(string newLanguage)
  {
    // If the language is French, set the locale to French.
    if(newLanguage == "French")
    {
      var FrenchLocale = LocalizationSettings.AvailableLocales.GetLocale("fr");
      LocalizationSettings.SelectedLocale = FrenchLocale;
    }
    // If the language is English, set the locale to English.
    else if(newLanguage == "English")
    {
      var EnglishLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
      LocalizationSettings.SelectedLocale = EnglishLocale;
    }
  }
  /// <summary>
  /// Adjusts the text of the component based on the selected locale.
  /// This method is called when the selected locale changes.
  /// </summary>
  void AdjustText(Locale locale)
  {
    SetupLocalizationString();
  }
  private void OnDestroy()
  {
    LocalizationSettings.SelectedLocaleChanged -= AdjustText;
    localizedSTR.StringChanged -= UpdateText;
  }
}
