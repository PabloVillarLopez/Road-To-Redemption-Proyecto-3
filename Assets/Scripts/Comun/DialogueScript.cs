using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    #region Reference Variables

    [Header("Reference Variables")]
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public float textSpeed = 0.1f;
    public bool dialogueStarted;
    public bool dialogueFinished;

    #endregion Reference Variables

    #region Spanish Lines of Dialogue

    [Header("Lines of Dialogue In Spanish")]
    [Tooltip("Put the dialogue lines in Spanish")]
    [TextArea(3,4)]
    public string[] spanishLines;
    private int spanishIndex;

    #endregion Spanish Lines of Dialogue

    #region English Lines of Dialogue

    [Header("Lines of Dialogue In English")]
    [Tooltip("Put the dialogue lines in English")]
    [TextArea(3,4)]
    public string[] englishLines;
    private int englishIndex;

    #endregion English Lines of Dialogue

    // Start is called before the first frame update
    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        //Spanish Dialogues

        if (Input.GetKeyDown(KeyCode.N) && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            if (dialogueText.text == spanishLines[spanishIndex])
            {
                NextSpanishLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = spanishLines[spanishIndex];
                dialogueFinished = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            if (dialogueText.text == spanishLines[spanishIndex])
            {
                PreviousSpanishLine();
            }
        }

        //English Dialogues

        if (Input.GetKeyDown(KeyCode.N) && LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            if (dialogueText.text == englishLines[englishIndex])
            {
                NextEnglishLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = englishLines[englishIndex];
                dialogueFinished = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            if (dialogueText.text == englishLines[englishIndex])
            {
                PreviousEnglishLine();
            }
        }
    }

    #region Spanish Dialogue System

    public void StartSpanishDialogue()
    {
        spanishIndex = 0;
        dialogueText.text = string.Empty;
        dialoguePanel.SetActive(true);
        dialogueStarted = true;
        dialogueFinished = false;

        StartCoroutine(WriteSpanishLine());
    }

    IEnumerator WriteSpanishLine()
    {
        foreach (var letter in spanishLines[spanishIndex].ToCharArray())
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextSpanishLine()
    {
        if (spanishIndex < spanishLines.Length - 1)
        {
            spanishIndex++;
            dialogueText.text = string.Empty;
            StartCoroutine(WriteSpanishLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
            dialogueStarted = false;
        }
    }

    public void PreviousSpanishLine()
    {
        if (spanishIndex > 0)
        {
            spanishIndex--;
            dialogueText.text = string.Empty;
            StartCoroutine(WriteSpanishLine());
        }
    }

    #endregion Spanish Dialogue System

    #region English Dialogue System

    public void StartEnglishDialogue()
    {
        englishIndex = 0;
        dialogueText.text = string.Empty;
        dialoguePanel.SetActive(true);
        dialogueStarted = true;
        dialogueFinished = false;

        StartCoroutine(WriteEnglishLine());
    }

    IEnumerator WriteEnglishLine()
    {
        foreach (var letter in englishLines[englishIndex].ToCharArray())
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextEnglishLine()
    {
        if (englishIndex < englishLines.Length - 1)
        {
            englishIndex++;
            dialogueText.text = string.Empty;
            StartCoroutine(WriteEnglishLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
            dialogueStarted = false;
        }
    }

    public void PreviousEnglishLine()
    {
        if (englishIndex > 0)
        {
            englishIndex--;
            dialogueText.text = string.Empty;
            StartCoroutine(WriteEnglishLine());
        }
    }

    #endregion English Dialogue System
}
