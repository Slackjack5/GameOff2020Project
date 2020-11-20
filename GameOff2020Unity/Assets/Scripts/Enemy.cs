using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyLaser laser;
    [SerializeField] private float shootSpeed = 12f;
    [Range(-180f, 180f)] [SerializeField] private float shootAngle = -90f;  // Angle relative to the positive x-axis
    [SerializeField] private float shootOffset = 0.5f;                      // Specifies how far from the enemy the projectile spawns
    [SerializeField] private float cooldown = 1.2f;                         // Specifies how long the enemy waits before shooting another projectile

    private float cooldownTime;

    // Start is called before the first frame update
    void Start()
    {
        cooldownTime = cooldown;
    }

    private void FixedUpdate()
    {
        cooldownTime -= Time.fixedDeltaTime;

        if (cooldownTime <= 0)
        {
            Shoot();
            cooldownTime = cooldown;
        }
    }

    private void Shoot()
    {
        // Spawn a projectile at a position near the enemy based on the shootOffset,
        // and rotate it based on the shootAngle.
        Quaternion rotation = Quaternion.AngleAxis(shootAngle, Vector3.forward);
        Vector3 spawnPosition = transform.position + rotation * Vector3.right * shootOffset;
        EnemyLaser shotLaser = Instantiate(laser, spawnPosition, rotation);
        shotLaser.SetShootSpeed(shootSpeed);
    }
}
