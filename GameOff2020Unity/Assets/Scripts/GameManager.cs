using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool playerIsDead = false;

    // These values represent the state of the current level
    public static bool levelComplete { get; set; }
    public static TimeMedal timeMedal { get; set; }
    public static float bestTime { get; set; }
    public static List<Vector2> ghostPositions { get; set; }

    private class Results
    {
        public bool levelComplete;
        public TimeMedal timeMedal;
        public float bestTime;
        public List<Vector2> ghostPositions;
    }

    // Store the results of each level
    private Results[] results;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        results = new Results[SceneManager.sceneCountInBuildSettings];

        Clear();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SaveResults();
        Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void SaveResults()
    {
        Results theResults = new Results
        {
            levelComplete = levelComplete,
            timeMedal = timeMedal,
            bestTime = bestTime,
            ghostPositions = ghostPositions
        };

        results[SceneManager.GetActiveScene().buildIndex] = theResults;
    }

    public void Clear()
    {
        levelComplete = false;
        bestTime = 0;
        ghostPositions = new List<Vector2>();
        timeMedal = TimeMedal.Unknown;
    }
}
