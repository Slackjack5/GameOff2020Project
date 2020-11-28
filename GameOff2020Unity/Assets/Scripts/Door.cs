using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AllEnemiesDead())
        {
            spriteRenderer.enabled = false;
            boxCollider.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;
        }
    }

    private bool AllEnemiesDead()
    {
        foreach (Enemy enemy in enemies)
        {
            if (!enemy.dead)
            {
                return false;
            }
        }

        return true;
    }
}
