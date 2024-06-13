using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public enum Language{
        Spanish,
        English
    }

    public static Language currentLanguage = Language.English;

    public void ChangeLanguageToSpanish()
    {
        currentLanguage = Language.Spanish;
    }

    public void ChangeLanguageToEnglish()
    {
        currentLanguage = Language.English;
    }
}
