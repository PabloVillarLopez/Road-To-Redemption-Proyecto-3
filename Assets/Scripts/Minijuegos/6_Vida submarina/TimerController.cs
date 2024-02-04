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

    private void Awake()
    {
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
            }

            int timeMin = Mathf.FloorToInt(timeLeft / 60);
            int timeSec = Mathf.FloorToInt(timeLeft % 60);

            timeText.text = string.Format("{00:00}:{01:00}", timeMin, timeSec);
        }
    }
}
