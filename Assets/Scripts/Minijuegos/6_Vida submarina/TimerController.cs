using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    [Header("Timer / Temporizador")]
    public int min;
    public int sec;
    public TMP_Text timeText;

    private float timeLeft;
    private bool timerOn = true;

    [Header("References / Referencias")]
    public GameObject sellosPanelPanel;
    public GameObject sellosPanel;
    public GameObject gameOverPanel;
    public GameObject pauseIndicatorEnglish;
    public GameObject pauseIndicatorSpanish;
    public GameObject initialInstructionsEnglish;
    public GameObject initialInstructionsSpanish;
    public GameObject tutorialEnglish;
    public GameObject tutorialSpanish;
    public GameObject finalMessageEnglish;
    public GameObject finalMessageSpanish;
    public GameObject robotIcon;
    public GameObject instructionsIndicatorEnglish;
    public GameObject instructionsIndicatorSpanish;
    private bool canShowInstructions = false;

    [Header("Sound Variables")]
    public List<AudioClip> soundClips = new List<AudioClip>(); // Lista de clips de sonido
    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioSource audioSourceBackground;

    private void Awake()
    {
        sellosPanelPanel.SetActive(false);
        sellosPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        tutorialEnglish.SetActive(false);
        tutorialSpanish.SetActive(false);
        finalMessageEnglish.SetActive(false);
        finalMessageSpanish.SetActive(false);
        robotIcon.SetActive(false);
        instructionsIndicatorEnglish.SetActive(false);
        instructionsIndicatorSpanish.SetActive(false);

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            
            initialInstructionsEnglish.SetActive(true);
            initialInstructionsSpanish.SetActive(false);   
        }

        if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            
            initialInstructionsEnglish.SetActive(false);
            initialInstructionsSpanish.SetActive(true);
        }

        timeLeft = (min * 60) + sec;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        MouseLook.canLook = false;
        ObserveObject.cantMove = true;
        PauseMenuManager.canPause = false;
        pauseIndicatorEnglish.SetActive(false);
        pauseIndicatorSpanish.SetActive(false);

        if (audioSourceBackground != null)
        {
            audioSourceBackground.clip = soundClips[0];
            audioSourceBackground.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowInstructions();

        if (timerOn == true)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                timerOn = false;
                if (ShootTrash.points < 31)
                {
                    Debug.Log("Te faltan puntos. Inténtalo de nuevo");
                    gameOverPanel.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    MinigamesCompleted.minigame3Finished = true;
                    //StartCoroutine(Wait());
                    FinishGame();
                }
                
            }

            int timeMin = Mathf.FloorToInt(timeLeft / 60);
            int timeSec = Mathf.FloorToInt(timeLeft % 60);

            timeText.text = string.Format("{00:00}:{01:00}", timeMin, timeSec);
        }

        if (ShootTrash.points >= 31)
        {
            timerOn = false;
            FinishGame();
            //MinigamesCompleted.minigame6Finished = true;
            //StartCoroutine(Wait());

        }
    }

    private IEnumerator Wait()
    {
        sellosPanelPanel.SetActive(true);
        sellosPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("LevelSelector");
    }

    public void Restart()
    {
        PlaySound(3);
        SceneManager.LoadScene("Minijuego6_VidaSubmarina");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToLevelSelector()
    {
        PlaySound(3);
        SceneManager.LoadScene("LevelSelector");
    }

    public void ShowTutorialPanel()
    {
        PlaySound(3);

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            initialInstructionsEnglish.SetActive(false);
            tutorialEnglish.SetActive(true);
            tutorialSpanish.SetActive(false);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            initialInstructionsSpanish.SetActive(false);
            tutorialEnglish.SetActive(false);
            tutorialSpanish.SetActive(true);
        }
    }

    public void HideTutorials()
    {
        PlaySound(3);
        tutorialEnglish.SetActive(false);
        tutorialSpanish.SetActive(false);
        robotIcon.SetActive(true);
        canShowInstructions = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MouseLook.canLook = true;
        ObserveObject.cantMove = false;
        PauseMenuManager.canPause = true;

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            pauseIndicatorEnglish.SetActive(true);
            pauseIndicatorSpanish.SetActive(false);
            instructionsIndicatorEnglish.SetActive(true);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            pauseIndicatorEnglish.SetActive(false);
            pauseIndicatorSpanish.SetActive(true);
            instructionsIndicatorSpanish.SetActive(true);
        }
        
    }

    private void FinishGame()
    {
        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            finalMessageEnglish.SetActive(true);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            finalMessageSpanish.SetActive(true);
        }
    }

    public void FinishMinigame()
    {
        PlaySound(3);
        MinigamesCompleted.minigame6Finished = true;
        SceneManager.LoadScene("LevelSelector");
    }

    public void PlaySound(int sound)
    {
        if (audioSource != null)
        {
            audioSource.clip = soundClips[sound];
            audioSource.Play();
        }

    }

    private void ShowInstructions()
    {
        if (Input.GetKeyDown(KeyCode.E) && canShowInstructions)
        {
            robotIcon.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MouseLook.canLook = false;
            ObserveObject.cantMove = true;
            PauseMenuManager.canPause = false;

            if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                initialInstructionsEnglish.SetActive(true);
                instructionsIndicatorEnglish.SetActive(false);
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                initialInstructionsSpanish.SetActive(true);
                instructionsIndicatorSpanish.SetActive(false);
            }

            canShowInstructions = false;
        }
    }
}
