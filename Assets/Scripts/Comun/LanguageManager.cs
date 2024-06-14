using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public GameObject panelLanguageChangedToEnglish;
    public GameObject panelLanguageChangedToSpanish;

    public enum Language{
        Spanish,
        English
    }

    public static Language currentLanguage = Language.English;

    public void ChangeLanguageToSpanish()
    {
        currentLanguage = Language.Spanish;
    }

    public void ChangeLanguageToSpanishAndFeedback()
    {
        currentLanguage = Language.Spanish;
        StartCoroutine(ShowSpanishFeedbackPanel());
    }

    public void ChangeLanguageToEnglish()
    {
        currentLanguage = Language.English;
    }

    public void ChangeLanguageToEnglishAndFeedback()
    {
        currentLanguage = Language.Spanish;
        StartCoroutine(ShowEnglishFeedbackPanel());
    }

    public IEnumerator ShowSpanishFeedbackPanel()
    {
        panelLanguageChangedToEnglish.SetActive(false);
        panelLanguageChangedToSpanish.SetActive(true);
        yield return new WaitForSeconds(2f);
        panelLanguageChangedToSpanish.SetActive(false);
    }

    public IEnumerator ShowEnglishFeedbackPanel()
    {
        panelLanguageChangedToSpanish.SetActive(false);
        panelLanguageChangedToEnglish.SetActive(true);
        yield return new WaitForSeconds(2f);
        panelLanguageChangedToEnglish.SetActive(false);
    }
}
