using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    [SerializeField] private float hoverDistance = 0.2f;  // Specifies how far up or down from the center to hover
    [SerializeField] private float hoverSpeed = 3.5f;     // Specifies how fast to move up and down
    [SerializeField] private float deathTime = 1.5f;      // Specifies how long the enemy stays in existence after being killed before disappearing
    [SerializeField] private int styleReward = 25;
    [SerializeField] private Style style;

    private Rigidbody2D rb;
    private Vector2 startPosition; 

    public bool dead { get; private set; }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
    }

    protected virtual void FixedUpdate()
    {
        if (!dead)
        {
            Hover();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon" && !dead)
        {
            Die();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Weapon" && !dead)
        {
            Die();
        }
    }

    private void Hover()
    {
        float newPositionY = Mathf.Sin(Time.timeSinceLevelLoad * hoverSpeed) * hoverDistance;
        transform.position = new Vector2(transform.position.x, startPosition.y + newPositionY);
    }

    public virtual void Die()
    {
        dead = true;

        style.AddStyle(styleReward);

        // Drop to the ground
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 4;

        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), player.GetComponent<BoxCollider2D>());

        //Play Sound
        FindObjectOfType<AudioManager>().PlaySound("EnemyDeath", Random.Range(.95f, 1f));

        //Play Random Cart Clear Voice Line
        int rand = Random.Range(0, 7);
        //Chance of Voice Line on Death 
        int rand2 = Random.Range(0, 4);
        //Play Voice Line
        if (rand2 == 1)
        {
            if (rand == 0)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-EnemyKilled1", Random.Range(.95f, 1f));
            }
            else if (rand == 1)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-EnemyKilled2", Random.Range(.95f, 1f));
            }
            else if (rand == 2)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-EnemyKilled3", Random.Range(.95f, 1f));
            }
            else if (rand == 3)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-EnemyKilled4", Random.Range(.95f, 1f));
            }
            else if (rand == 4)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-EnemyKilled5", Random.Range(.95f, 1f));
            }
            else if (rand == 5)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-EnemyKilled6", Random.Range(.95f, 1f));
            }
            else if (rand == 6)
            {
                //Play Sound
                FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-EnemyKilled7", Random.Range(.95f, 1f));
            }
        }

        StartCoroutine(Drop());
    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(deathTime);
        gameObject.SetActive(false);
    }
}
