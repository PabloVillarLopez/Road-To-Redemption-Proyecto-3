using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnalyzeClue : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static float timer = 0;
    public static int cluesAnalyzed;
    public bool buttonPressed;
    public Slider analyzeSlider;
    public GameObject analyzeButton;
    public ObserveObject minigameManager;
    public GameObject holdButtonIndicatorEnglish;
    public GameObject holdButtonIndicatorSpanish;

    private void Awake()
    {
        holdButtonIndicatorEnglish.SetActive(false);
        holdButtonIndicatorSpanish.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed && timer < 5)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
        }

        if (timer >= 5)
        {
            cluesAnalyzed++;

            switch (cluesAnalyzed)
            {
                case 1:
                    if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                    {
                        minigameManager.note0English.SetActive(false);
                        minigameManager.note1English.SetActive(true);
                    }
                    else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                    {
                        minigameManager.note0Spanish.SetActive(false);
                        minigameManager.note1Spanish.SetActive(true);
                    }
                    break;
                case 2:
                    if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                    {
                        minigameManager.note1English.SetActive(false);
                        minigameManager.note2English.SetActive(true);
                    }
                    else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                    {
                        minigameManager.note1Spanish.SetActive(false);
                        minigameManager.note2Spanish.SetActive(true);
                    }
                    break;
                case 3:
                    if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                    {
                        minigameManager.note2English.SetActive(false);
                        minigameManager.note3English.SetActive(true);
                    }
                    else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                    {
                        minigameManager.note2Spanish.SetActive(false);
                        minigameManager.note3Spanish.SetActive(true);
                    }
                    break;
                default:
                    break;
            }

            

            
            Debug.Log(cluesAnalyzed);
            analyzeSlider.gameObject.SetActive(false);
            analyzeButton.SetActive(false);

            if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                holdButtonIndicatorEnglish.SetActive(false);
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                holdButtonIndicatorSpanish.SetActive(false);
            }

            buttonPressed = false;
            timer = 0;
            minigameManager.PlaySound(4);
        }

        if (!buttonPressed)
        {
            timer = 0;
        }

        analyzeSlider.value = timer;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
