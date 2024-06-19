using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCollideMagGlass : MonoBehaviour
{
    public GameObject magnifyingGlass;
    public GameObject pressLeftClickIndicatorEnglish;
    public GameObject pressLeftClickIndicatorSpanish;

    private void Start()
    {
        HideInteractors();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Clue1Area") ||
            !other.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Clue2Area") ||
            !other.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Clue3Area"))
        {
            magnifyingGlass.SetActive(true);
            CheckLanguageAndShowInteractor();
        }

        //magnifyingGlass.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Clue1Area") ||
            !other.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Clue2Area") ||
            !other.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Clue3Area"))
        {
            magnifyingGlass.SetActive(false);
            HideInteractors();
        }

        //magnifyingGlass.SetActive(true);
    }

    private void CheckLanguageAndShowInteractor()
    {
        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            pressLeftClickIndicatorEnglish.SetActive(true);
            pressLeftClickIndicatorSpanish.SetActive(false);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            pressLeftClickIndicatorEnglish.SetActive(false);
            pressLeftClickIndicatorSpanish.SetActive(true);
        }
    }

    private void HideInteractors()
    {
        pressLeftClickIndicatorEnglish.SetActive(false);
        pressLeftClickIndicatorSpanish.SetActive(false);
    }
}
