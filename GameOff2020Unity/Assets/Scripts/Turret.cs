using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : AggressiveEnemy
{
    [SerializeField] private GameObject chargeCircle;
    [SerializeField] private float chargeTime = 0.7f;
    [SerializeField] private float cooldown = 0.7f;  // Specifies how long the enemy waits before shooting another projectile

    private float cooldownTime;
    private GameObject newChargeCircle;
    private float currentChargeTime;
    private bool charging;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        cooldownTime = cooldown;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!dead)
        {
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

            // Check if player is within range
            if (playerWithinRange)
            {
                if (cooldownTime <= 0 && !charging && !GameManager.playerIsDead)
                {
                    ChargeLaser();
                }
            }
        }
    }

    private void ChargeLaser()
    {
        newChargeCircle = Instantiate(chargeCircle, transform.position, Quaternion.identity, transform);
        currentChargeTime = chargeTime;
        charging = true;
        //Play Charging Sound
        FindObjectOfType<AudioManager>().PlaySound("LaserCharge", Random.Range(.95f, 1f));
    }

    protected override void Shoot()
    {
        base.Shoot();

        Destroy(newChargeCircle);
        charging = false;
        cooldownTime = cooldown;
    }

    public override void Die()
    {
        if (charging)
        {
            FindObjectOfType<AudioManager>().Stop("LaserCharge");
            Destroy(newChargeCircle);
        }

        base.Die();
    }
}
