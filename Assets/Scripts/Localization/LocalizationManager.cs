using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using TMPro;

public class LocalizationManager : MonoBehaviour
{
  [Header("Localization Settings")]
  [SerializeField] private List<LocalizationComponent> localizationComponents; // List of localization components to update.
  [SerializeField] private TMP_Dropdown languageDropdown; // Dropdown for selecting the language.
  [SerializeField] private string localizationTableName; // The name of the localization table.
  [SerializeField] private string selectedLocale; // The currently selected locale.
  [SerializeField] private string localizationKey; // The key used to identify the text in the localization table.

  private void Start()
  {
    // Populate the dropdown with available locales.
    PopulateLanguageDropdown();

    // Add listener to handle dropdown value changes.
    languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    foreach (var component in localizationComponents)
    {
      component.localizationTableName = localizationTableName;
      component.SetLanguage(selectedLocale);
    }
  }

  /// <summary>
  /// Populate the language dropdown with available locales.
  /// </summary>
  private void PopulateLanguageDropdown()
  {
    languageDropdown.options.Clear();

    foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
    {
      languageDropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
    }

    // Set the dropdown to the currently selected locale.
    var currentLocale = LocalizationSettings.SelectedLocale;
    languageDropdown.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(currentLocale);
    languageDropdown.RefreshShownValue();
  }

  /// <summary>
  /// Sets the language for the localization system.
  /// This method changes the selected locale based on the provided language string.
  /// </summary>
  /// <param name="newLanguage"></param>
  public void SetLanguage(string newLanguage)
  {
    var locale = LocalizationSettings.AvailableLocales.GetLocale(newLanguage);
    if (locale != null)
    {
      LocalizationSettings.SelectedLocale = locale;

      selectedLocale = newLanguage;

      // Update all localization components with the new language.
      foreach (var component in localizationComponents)
      {
        component.localizationTableName = localizationTableName;
        component.SetLanguage(newLanguage);
      }
    }
  }

  /// <summary>
  /// Changes the locale based on the selected value in the dropdown.
  /// This method is called when the dropdown value changes.
  /// </summary>
  /// <param name="index"></param>
  public void OnLanguageChanged(int index)
  {
    if(index == 0)
    {
      // If the first option is selected, set the language to English.
      SetLanguage("en");
    }
    else if(index == 1)
    {
      // If the second option is selected, set the language to French.
      SetLanguage("fr");
    }
    // Get the selected language from the dropdown.
    string selectedLanguage = languageDropdown.options[index].text;

    // Set the language based on the selected value.
    SetLanguage(selectedLanguage);
  }
}
