using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    private float timeElapsed = 0;
    private bool timerIsRunning = false;

    private TextMeshProUGUI timeText;

    private void Start()
    {
        timerIsRunning = true;
        timeText = gameObject.GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (timerIsRunning)
        {
            timeElapsed += Time.deltaTime;
            DisplayTime(timeElapsed);
        }
        else
        {
            Debug.Log("Time has run out!");
            timeElapsed = 0;
            timerIsRunning = false;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = Mathf.FloorToInt((timeToDisplay * 100) % 100);

        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);
    }

}
