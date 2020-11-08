using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private List<Vector2> playerPositions;
    private List<Vector2> ghostPositions;
    private int ghostPositionIndex = 0;
    private bool reset = false;
    private bool showGhost = false;

    // Start is called before the first frame update
    void Start()
    {
        playerPositions = new List<Vector2>();
        ghostPositions = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetComponent<SpriteRenderer>().enabled = showGhost;

        // On reset, let the ghost know the path it should follow, 
        // and start recording a new list of the player's positions
        if (reset)
        {
            ghostPositions = playerPositions;
            ghostPositionIndex = 0;
            playerPositions = new List<Vector2>();
            reset = false;
        }
    }

    private void FixedUpdate()
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
