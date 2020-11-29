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
    [SerializeField] private float travelTime = 0.4f;                      // Specifies how long it takes for the current time to travel to its position in the finish level menu
    [SerializeField] private RectTransform targetPositionTransform;
    [SerializeField] private TextMeshProUGUI previousTimeText;
    [SerializeField] private TextMeshProUGUI differenceText;
    [SerializeField] private Color fasterTimeColor;
    [SerializeField] private Color slowerTimeColor;
    [SerializeField] private float targetTime = 30f;                       // The time the player should beat to beat this level
    [SerializeField] private TextMeshProUGUI targetTimeText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button continueButton;
    [SerializeField] private string successMessage;                        // The message that is shown to the player when they beat the time
    [SerializeField] private string failureMessage;                        // The message that is shown to the player when they fail to beat the time
    [SerializeField] private GameObject successGlow;

    private TextMeshProUGUI timerText;
    private RectTransform rectTransform;

    private float timeElapsed = 0;
    private bool timerIsRunning = false;
    private bool countingDown = true;
    private int countdownTime = 3;       // What is displayed
    private float currentCountdownTime;  // Represents how long before countdownTime decrements by 1
    private bool levelFinished = false;
    private Vector2 anchorVelocity = Vector2.zero;
    private Vector2 anchoredPositionVelocity = Vector2.zero;
    private float previousTimeElapsed;
    private float difference = 0;        // Difference between the current time elapsed and the previous time elapsed

    private readonly Vector2 centerAnchor = new Vector2(0.5f, 0.5f);

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        currentCountdownTime = countdownSecond;

        previousTimeElapsed = GameManager.previousTimeElapsed;
        previousTimeText.text = "--";
        differenceText.text = "--";

        targetTimeText.text = FormatTime(targetTime);

        GameManager.playerIsDead = true;
    }

    private void Update()
    {
        timerText.text = FormatTime(timeElapsed);
        
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

        if (levelFinished)
        {
            MoveTimer();
        }
    }

    private string FormatTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = Mathf.FloorToInt((timeToDisplay * 100) % 100);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);
    }

    private void DisplayCountdownTime(int time)
    {
        countdownText.enabled = true;

        // Have the countdown get smaller and more transparent over time
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

    public void FinishTime()
    {
        timerIsRunning = false;
        GameManager.playerIsDead = true;
        levelFinished = true;
        
        // Compare the previous time to the current time
        if (previousTimeElapsed > 0)
        {
            previousTimeText.text = FormatTime(previousTimeElapsed);

            difference = timeElapsed - previousTimeElapsed;
            string prefix;
            if (difference < 0)
            {
                differenceText.color = fasterTimeColor;
                difference = -difference;
                prefix = "-";
            }
            else
            {
                differenceText.color = slowerTimeColor;
                prefix = "+";
            }

            differenceText.text = prefix + FormatTime(difference);
        }

        // Show whether the player beat the target time
        bool levelComplete = timeElapsed < targetTime || GameManager.levelComplete;
        continueButton.interactable = levelComplete;
        if (levelComplete)
        {
            resultText.text = successMessage;
            successGlow.SetActive(true);
            GameManager.levelComplete = true;
        }
        else
        {
            resultText.text = failureMessage;
            successGlow.SetActive(false);
        }

        GameManager.previousTimeElapsed = timeElapsed;
    }

    private void MoveTimer()
    {
        // Move the anchor to the center over time
        Vector2 currentAnchor = rectTransform.anchorMin;
        Vector2 newAnchor = Vector2.SmoothDamp(currentAnchor, centerAnchor, ref anchorVelocity, travelTime);
        rectTransform.anchorMin = newAnchor;
        rectTransform.anchorMax = newAnchor;

        // Move the anchored position to the side over time
        Vector2 currentAnchoredPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = Vector2.SmoothDamp(currentAnchoredPosition, targetPositionTransform.anchoredPosition, ref anchoredPositionVelocity, travelTime);
    }
}
