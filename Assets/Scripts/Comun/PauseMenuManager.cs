using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("To Decide When the Player Can Pause The Game")]
    public static bool canPause = true;
    [Header("Pause Panel / Panel de pausa")]
    public GameObject spanishPausePanel;
    public GameObject englishPausePanel;
    public static bool gameIsPaused;

    // Start is called before the first frame update
    void Start()
    {
        spanishPausePanel.SetActive(false);
        englishPausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && LanguageManager.currentLanguage == LanguageManager.Language.Spanish && !spanishPausePanel.activeInHierarchy)
        {
            Time.timeScale = 0f;
            spanishPausePanel.SetActive(true);
            gameIsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && LanguageManager.currentLanguage == LanguageManager.Language.English && !englishPausePanel.activeInHierarchy)
        {
            Time.timeScale = 0f;
            englishPausePanel.SetActive(true);
            gameIsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void ContinueGame()
    {
        englishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ContinuarJuego()
    {
        spanishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ContinueGameMinigame1()
    {
        englishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        ManageCursorMinigame1();
    }

    public void ContinuarJuegoMinijuego1()
    {
        spanishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        ManageCursorMinigame1();
    }

    public void ManageCursorMinigame1()
    {
        if (ObserveObject.phases == ObserveObject.JusticePhases.INVESTIGATION)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (ObserveObject.phases == ObserveObject.JusticePhases.ANALYSIS)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (ObserveObject.phases == ObserveObject.JusticePhases.JUDGMENT)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
