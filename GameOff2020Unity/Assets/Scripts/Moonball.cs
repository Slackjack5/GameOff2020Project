using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonball : MonoBehaviour
{
    public float shootSpeed { get; set; }

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * shootSpeed;
    }

    private void Update()
    {
        if (GameManager.playerIsDead)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Kill enemy on hit
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Die();

            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            // Recharge the player
            Destroy(gameObject);
        }
        else
        {
            //Play Sound
            FindObjectOfType<AudioManager>().PlaySound("LaserDeflect", Random.Range(.95f, 1f));
        }
    }
}
