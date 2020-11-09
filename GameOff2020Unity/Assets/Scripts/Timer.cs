using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    
    public float timeRemaining = 0;
    public bool timerIsRunning = false;

    public TextMeshProUGUI timeText;
    private void Start()
    {
        timerIsRunning = true;
        //timeText = gameObject.GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (timerIsRunning)
        {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
        }
        else
        {
            Debug.Log("Time has run out!");
            timeRemaining = 0;
            timerIsRunning = false;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = (timeToDisplay % 1) * 100;

        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);
    }

}
