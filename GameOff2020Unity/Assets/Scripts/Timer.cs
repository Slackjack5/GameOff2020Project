using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float countdownSecond = 1.0f;  // Specifies how long it takes to decrement the countdown by 1
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float maxFontSize = 60f;
    [SerializeField] private float minFontSize = 24f;
    
    private TextMeshProUGUI timerText;

    private float timeElapsed = 0;
    private bool timerIsRunning = false;
    private bool countingDown = true;
    private int countdownTime = 3;       // What is displayed
    private float currentCountdownTime;  // Represents how long before countdownTime decrements by 1

    private void Start()
    {
        timerText = gameObject.GetComponent<TextMeshProUGUI>();

        currentCountdownTime = countdownSecond;
        GameManager.playerIsDead = true;
    }

    private void Update()
    {
        DisplayTime(timeElapsed);
        
        if (countingDown)
        {
            DisplayCountdownTime(countdownTime);

            currentCountdownTime -= Time.deltaTime;
            if (currentCountdownTime <= 0)
            {
                countdownTime--;
                currentCountdownTime = countdownSecond;
            }

            if (countdownTime <= 0)
            {
                // Countdown is over
                StartCoroutine(DisplayGo());

                countingDown = false;
                timerIsRunning = true;
                GameManager.playerIsDead = false;
            }
        }

        if (timerIsRunning)
        {
            timeElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed = 0;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = Mathf.FloorToInt((timeToDisplay * 100) % 100);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);
    }

    private void DisplayCountdownTime(int time)
    {
        float interpolationValue = Mathf.InverseLerp(0, countdownSecond, currentCountdownTime);

        countdownText.fontSize = Mathf.Lerp(minFontSize, maxFontSize, interpolationValue);

        Color color = countdownText.color;
        color.a = interpolationValue;
        countdownText.color = color;

        countdownText.text = string.Format("{0}", time);
    }

    private IEnumerator DisplayGo()
    {
        countdownText.fontSize = maxFontSize;

        Color color = countdownText.color;
        color.a = 1;
        countdownText.color = color;

        countdownText.text = "GO";

        yield return new WaitForSeconds(countdownSecond);

        countdownText.enabled = false;
    }
}
