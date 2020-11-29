using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : Enemy
{
    [SerializeField] protected Laser laser;
    [SerializeField] protected float shootTime = 0.5f;
    [SerializeField] protected float shootOffset = 0.8f;       // Specifies how far from the enemy the projectile spawns
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float detectionRadius = 12.0f;  // Radius of the circle to detect if the player is within range
    [SerializeField] private float rotationSpeed = 0.2f;     // The speed at which the enemy rotates to lock on to the player

    protected bool playerWithinRange;
    private float rotationTime;
    protected bool shooting;
    private float currentShootTime;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!dead)
        {
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
            playerWithinRange = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayerMask);
            if (playerWithinRange)
            {
                // While the enemy is shooting the laser, don't rotate the enemy
                if (!shooting)
                {
                    FacePlayer();
                }
            }
        }
    }

    protected void FacePlayer()
    {
        Vector3 targetDirection = player.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, rotationZ), rotationTime + rotationSpeed);
    }

    protected virtual void Shoot()
    {
        // Spawn a projectile at a position near the enemy based on the shootOffset
        Vector3 spawnPosition = transform.position + transform.rotation * Vector3.right * shootOffset;
        Laser shotLaser = Instantiate(laser, spawnPosition, transform.rotation);
        shotLaser.shootTime = shootTime;

        currentShootTime = shootTime;
        shooting = true;

        //Play Shoot Sound
        FindObjectOfType<AudioManager>().PlaySound("LaserShoot", Random.Range(.95f, 1f));
        FindObjectOfType<AudioManager>().Stop("LaserCharge");
    }

    public override void Respawn()
    {
        currentShootTime = 0;
        shooting = false;

        base.Respawn();
    }
}
