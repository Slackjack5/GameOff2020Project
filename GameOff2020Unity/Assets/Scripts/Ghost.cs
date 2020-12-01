using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Timer timer;

    private Vector2 startPosition;
    private List<Vector2> playerPositions;
    private List<Vector2> ghostPositions;
    private int ghostPositionIndex = 0;
    private bool activated = true;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        playerPositions = new List<Vector2>();
        ghostPositions = GameManager.ghostPositions;

        if (ghostPositions.Count == 0)
        {
            transform.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (activated)
        {
            // Move the ghost to its next position
            if (ghostPositionIndex < ghostPositions.Count)
            {
                transform.position = ghostPositions[ghostPositionIndex];
                ghostPositionIndex++;
            }

            // Snapshot the player's position
            playerPositions.Add(player.transform.position);
        }
    }

    public void SaveRecording()
    {
        // Stop recording
        activated = false;

        transform.GetComponent<SpriteRenderer>().enabled = false;
        transform.position = startPosition;

        // On reset, let the ghost know the path it should follow, 
        // and start recording a new list of the player's positions
        if (GameManager.bestTime == 0 || timer.timeElapsed < GameManager.bestTime)
        {
            GameManager.ghostPositions = playerPositions;
        }
    }
}
