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

    // Start is called before the first frame update
    void Start()
    {
        spanishPausePanel.SetActive(false);
        englishPausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            spanishPausePanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            englishPausePanel.SetActive(true);
        }
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("LevelSelector");
    }
}
