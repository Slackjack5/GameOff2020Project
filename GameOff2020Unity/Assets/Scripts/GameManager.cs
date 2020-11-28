using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool playerIsDead = false;

    private GameObject[] goals;
    private GameObject[] enemies;

    private void Start()
    {
        goals = GameObject.FindGameObjectsWithTag("Goal");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void RestartLevel()
    {
        foreach (GameObject goal in goals)
        {
            goal.GetComponent<Goal>().Reset();
        }

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().Respawn();
        }
    }
}
