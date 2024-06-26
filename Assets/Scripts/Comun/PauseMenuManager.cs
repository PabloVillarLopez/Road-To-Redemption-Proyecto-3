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

    [Header("Minigames Musics")]
    public AudioSource minigame1Music;
    public AudioSource minigame3Music;
    public AudioSource minigame5Music;
    public AudioSource minigame6Music;

    // Start is called before the first frame update
    void Start()
    {
        spanishPausePanel.SetActive(false);
        englishPausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && LanguageManager.currentLanguage == LanguageManager.Language.Spanish && !spanishPausePanel.activeInHierarchy && canPause)
        {
            ManagerToPauseMinigameMusic();
            Time.timeScale = 0f;
            spanishPausePanel.SetActive(true);
            gameIsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && LanguageManager.currentLanguage == LanguageManager.Language.English && !englishPausePanel.activeInHierarchy && canPause)
        {
            ManagerToPauseMinigameMusic();
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
        ManagerToContinueMinigameMusic();
        englishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ContinuarJuego()
    {
        ManagerToContinueMinigameMusic();
        spanishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ContinueGameMinigame1()
    {
        ManagerToContinueMinigameMusic();
        englishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        ManageCursorMinigame1();
    }

    public void ContinuarJuegoMinijuego1()
    {
        ManagerToContinueMinigameMusic();
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

    public void ContinueGameMinigame5()
    {
        ManagerToContinueMinigameMusic();
        englishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        ManageCursorMinigame5();
    }

    public void ContinuarJuegoMinijuego5()
    {
        ManagerToContinueMinigameMusic();
        spanishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        ManageCursorMinigame5();
    }

    public void ManageCursorMinigame5()
    {
        if (energyMinigameManager.phase == energyMinigameManager.Phase.PHASE1)
        {
            if (canPause)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else if (energyMinigameManager.phase == energyMinigameManager.Phase.PHASE2)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (energyMinigameManager.phase == energyMinigameManager.Phase.PHASE3)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ContinueGameMinigame3()
    {
        ManagerToContinueMinigameMusic();
        englishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        ManageCursorMinigame3();
    }

    public void ContinuarJuegoMinijuego3()
    {
        ManagerToContinueMinigameMusic();
        spanishPausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        ManageCursorMinigame3();
    }

    public void ManageCursorMinigame3()
    {
        if (MiniGameManager.phases == MiniGameManager.Phases.TREATMENTPLANT)
        {
            if (canPause)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!canPause)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else if (MiniGameManager.phases == MiniGameManager.Phases.TOWN)
        {
            if (canPause)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!canPause)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void ManagerToPauseMinigameMusic()
    {
        if (minigame1Music != null)
        {
            minigame1Music.Stop();
        }

        if (minigame3Music != null)
        {
            minigame3Music.Stop();
        }

        if (minigame5Music != null)
        {
            minigame5Music.Stop();
        }

        if (minigame6Music != null)
        {
            minigame6Music.Stop();
        }
    }

    private void ManagerToContinueMinigameMusic()
    {
        if (minigame1Music != null)
        {
            minigame1Music.Play();
        }

        if (minigame3Music != null)
        {
            minigame3Music.Play();
        }

        if (minigame5Music != null)
        {
            minigame5Music.Play();
        }

        if (minigame6Music != null)
        {
            minigame6Music.Play();
        }
    }
}
