using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public int min, sec;
    public TMP_Text timeText;

    private float timeLeft;
    private bool timerOn = true;

    public GameObject sellosPanelPanel;
    public GameObject sellosPanel;
    public GameObject gameOverPanel;

    private void Awake()
    {
        sellosPanelPanel.SetActive(false);
        sellosPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        timeLeft = (min * 60) + sec;
    }

    // Update is called once per frame
    void Update()
    {
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
                    StartCoroutine(Wait());
                }
                
            }

            int timeMin = Mathf.FloorToInt(timeLeft / 60);
            int timeSec = Mathf.FloorToInt(timeLeft % 60);

            timeText.text = string.Format("{00:00}:{01:00}", timeMin, timeSec);
        }

        if (ShootTrash.points >= 31)
        {
            timerOn = false;
            MinigamesCompleted.minigame6Finished = true;
            StartCoroutine(Wait());
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
        SceneManager.LoadScene("Minijuego6_VidaSubmarina");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToLevelSelector()
    {
        SceneManager.LoadScene("LevelSelector");
    }
}
