using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Laser laser;
    [SerializeField] private GameObject chargeCircle;
    [SerializeField] private float chargeTime = 0.7f;
    [SerializeField] private float shootTime = 0.5f;
    [SerializeField] private float shootOffset = 0.8f;                      // Specifies how far from the enemy the projectile spawns
    [SerializeField] private float cooldown = 0.7f;                         // Specifies how long the enemy waits before shooting another projectile
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private GameObject player;
    [SerializeField] private float detectionRadius = 12.0f;                 // Radius of the circle to detect if the player is within range
    [SerializeField] private float rotationSpeed = 0.2f;                    // The speed at which the enemy rotates to lock on to the player
    [SerializeField] private float hoverDistance = 0.2f;                    // Specifies how far up or down from the center to hover
    [SerializeField] private float hoverSpeed = 3.5f;                       // Specifies how fast to move up and down
    [SerializeField] private float deathTime = 1.5f;                        // Specifies how long the enemy stays in existence after being killed before disappearing

    private Rigidbody2D rb;
    private float cooldownTime;
    private float rotationTime;
    private GameObject newChargeCircle;
    private float currentChargeTime;
    private bool charging;
    private float currentShootTime;
    private bool shooting;
    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cooldownTime = cooldown;
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            Hover();

            // Track cooldown
            cooldownTime -= Time.fixedDeltaTime;
            if (cooldownTime <= 0)
            {
                cooldownTime = 0;
            }

            // Track charge time
            if (charging)
            {
                currentChargeTime -= Time.fixedDeltaTime;
                if (currentChargeTime <= 0)
                {
                    Shoot();
                }
            }

            // Track shoot time
            if (shooting)
            {
                currentShootTime -= Time.fixedDeltaTime;
                if (currentShootTime <= 0)
                {
                    shooting = false;
                }
            }

            // Check if player is within range
            bool playerWithinRange = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayerMask);
            if (playerWithinRange)
            {
                // While the enemy is shooting the laser, don't rotate the enemy
                if (!shooting)
                {
                    FacePlayer();
                }

                if (cooldownTime <= 0 && !charging && !GameManager.playerIsDead)
                {
                    ChargeLaser();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            Die();
        }
    }

    private void Hover()
    {
        float newPositionY = Mathf.Sin(Time.timeSinceLevelLoad * hoverSpeed) * hoverDistance;
        transform.position = new Vector2(transform.position.x, newPositionY);
    }

    private void FacePlayer()
    {
        Vector3 targetDirection = player.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, rotationZ), rotationTime + rotationSpeed);
    }

    private void ChargeLaser()
    {
        newChargeCircle = Instantiate(chargeCircle, transform.position, Quaternion.identity, transform);
        currentChargeTime = chargeTime;
        charging = true;
        //Play Charging Sound
        FindObjectOfType<AudioManager>().PlaySound("LaserCharge", Random.Range(.95f, 1f));
    }

    private void Shoot()
    {
        // Spawn a projectile at a position near the enemy based on the shootOffset
        Vector3 spawnPosition = transform.position + transform.rotation * Vector3.right * shootOffset;
        Laser shotLaser = Instantiate(laser, spawnPosition, transform.rotation);
        shotLaser.shootTime = shootTime;

        currentShootTime = shootTime;
        shooting = true;

        Destroy(newChargeCircle);
        charging = false;
        cooldownTime = cooldown;

        //Play Shoot Sound
        FindObjectOfType<AudioManager>().PlaySound("LaserShoot", Random.Range(.95f, 1f));
        FindObjectOfType<AudioManager>().Stop("LaserCharge");
    }

    public void Die()
    {
        dead = true;

        // Drop to the ground
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 4;

        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());

        if (charging)
        {
            FindObjectOfType<AudioManager>().Stop("LaserCharge");
            Destroy(newChargeCircle);
        }

        StartCoroutine(Drop());
    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }
}
