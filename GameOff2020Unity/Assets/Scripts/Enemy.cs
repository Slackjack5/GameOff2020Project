using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyLaser laser;
    [SerializeField] private float shootSpeed = 24f;
    [SerializeField] private float shootOffset = 0.5f;                      // Specifies how far from the enemy the projectile spawns
    [SerializeField] private float cooldown = 1.2f;                         // Specifies how long the enemy waits before shooting another projectile
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float detectionRadius = 12.0f;                 // Radius of the circle to detect if the player is within range
    [SerializeField] private float rotationSpeed = 0.2f;                    // The speed at which the enemy rotates to lock on to the player
    [SerializeField] private float hoverDistance = 0.2f;                    // Specifies how far up or down from the center to hover
    [SerializeField] private float hoverSpeed = 3.5f;                       // Specifies how fast to move up and down

    private bool playerWithinRange = false;
    private bool playerPreviouslyWithinRange = false;
    private float cooldownTime;
    private float rotationTime;

    // Start is called before the first frame update
    void Start()
    {
        cooldownTime = cooldown;
    }

    private void FixedUpdate()
    {
        Hover();

        // Track cooldown
        cooldownTime -= Time.fixedDeltaTime;
        if (cooldownTime <= 0)
        {
            cooldownTime = 0;
        }

        // Check if player is within range
        playerWithinRange = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayerMask);
        if (playerWithinRange)
        {
            FacePlayer();
            if (!playerPreviouslyWithinRange)
            {
                // Give time for the enemy to face the player before shooting
                cooldownTime += rotationSpeed;
                playerPreviouslyWithinRange = true;
            }

            if (cooldownTime <= 0)
            {
                Shoot();
            }
        }
        else
        {
            playerPreviouslyWithinRange = false;
        }
    }

    private void Hover()
    {
        float newPositionY = Mathf.Sin(Time.timeSinceLevelLoad * hoverSpeed) * hoverDistance;
        transform.position = new Vector2(transform.position.x, newPositionY);
    }

    private void FacePlayer()
    {
        Vector3 targetDirection = playerTransform.position - transform.position;
        float rotationZ = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, rotationZ), rotationTime + rotationSpeed);
    }

    private void Shoot()
    {
        // Spawn a projectile at a position near the enemy based on the shootOffset
        Vector3 spawnPosition = transform.position + transform.rotation * Vector3.right * shootOffset;
        EnemyLaser shotLaser = Instantiate(laser, spawnPosition, transform.rotation);
        shotLaser.SetShootSpeed(shootSpeed);

        cooldownTime = cooldown;
    }
}
