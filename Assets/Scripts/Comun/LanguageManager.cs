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

    [Header("Sound Variables")]
    public List<AudioClip> soundClips = new List<AudioClip>(); // Lista de clips de sonido
    public AudioSource audioSource; // Referencia al componente AudioSource

    public void ChangeLanguageToSpanish()
    {
        currentLanguage = Language.Spanish;
    }

    public void ChangeLanguageToSpanishAndFeedback()
    {
        PlaySound(0);
        currentLanguage = Language.Spanish;
        StartCoroutine(ShowSpanishFeedbackPanel());
    }

    public void ChangeLanguageToEnglish()
    {
        currentLanguage = Language.English;
    }

    public void ChangeLanguageToEnglishAndFeedback()
    {
        PlaySound(0);
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

    public void PlaySound(int sound)
    {
        if (audioSource != null)
        {
            audioSource.clip = soundClips[sound];
            audioSource.Play();
        }

    }
}
