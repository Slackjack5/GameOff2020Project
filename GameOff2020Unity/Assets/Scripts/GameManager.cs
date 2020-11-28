using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool playerIsDead = false;

    private GameObject[] goals;
    private GameObject[] enemies;
    private GameObject[] entrances;
    private GameObject[] exits;

    private void Start()
    {
        goals = GameObject.FindGameObjectsWithTag("Goal");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        entrances = GameObject.FindGameObjectsWithTag("Entrance");
        exits = GameObject.FindGameObjectsWithTag("Exit");
    }

    public void RestartLevel()
    {
        foreach (GameObject goal in goals)
        {
            goal.GetComponent<Goal>().Reset();
        }

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Turret>().Respawn();
        }

        foreach (GameObject entrance in entrances)
        {
            entrance.GetComponent<EntranceDoor>().Open();
        }

        foreach (GameObject exit in exits)
        {
            exit.GetComponent<ExitDoor>().Unlock();
        }
    }
}
