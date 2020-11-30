using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float shootSpeed = 25f;
    [SerializeField] private float shootOffset = 1f;  // Specifies how far from the player the projectile spawns
    [SerializeField] private GameObject moonball;
    [SerializeField] private GameObject chargeCircle;
    [SerializeField] private Laser laser;
    [SerializeField] private float chargeTime = 0.7f;
    [SerializeField] private float shootTime = 0.5f;
    [SerializeField] private float laserCooldown = 0.7f;
    [SerializeField] private float reloadTime = 0.5f;

    private GameObject newMoonball;
    private bool reloading = false;

    public GameObject newChargeCircle { get; private set; }
    public bool chargingLaser { get; private set; }
    private float currentChargeTime;
    private bool shootingLaser;
    private float currentShootTime;
    private float laserCooldownTime;

    //Particle Effect
    public ParticleSystem myParticles;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.playerIsDead)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (newMoonball == null && !reloading)
                {
                    ShootBall();
                }
                else if (newMoonball != null)
                {
                    Reload();
                }
            }

            if (Input.GetButtonDown("Fire2") && !chargingLaser && laserCooldownTime <= 0)
            {
                ChargeLaser();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.playerIsDead)
        {
            // Track laser cooldown
            laserCooldownTime -= Time.fixedDeltaTime;
            if (laserCooldownTime <= 0)
            {
                laserCooldownTime = 0;
            }

            // Track laser charge time
            if (chargingLaser)
            {
                currentChargeTime -= Time.fixedDeltaTime;
                if (currentChargeTime <= 0)
                {
                    ShootLaser();
                }

            }

            // Track laser shoot time
            if (shootingLaser)
            {
                currentShootTime -= Time.fixedDeltaTime;
                if (currentShootTime <= 0)
                {
                    shootingLaser = false;
                }
            }
        }
    }

    private Vector2 PlayerToMouseDirection()
    {
        float distanceFromCameraToWorld = transform.position.z - Camera.main.transform.position.z;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCameraToWorld));
        Ray2D playerToMouse = new Ray2D(transform.position, mousePosition - transform.position);
        return playerToMouse.direction;
    }

    private void ShootBall()
    {
        // Spawn the projectile in the direction pointing towards the mouse
        Vector2 playerToMouseDirection = PlayerToMouseDirection();
        Vector2 transformPosition = transform.position;
        Vector2 spawnPosition = transformPosition + playerToMouseDirection * shootOffset;
        float shootAngle = Vector2.SignedAngle(Vector2.right, playerToMouseDirection);
        newMoonball = Instantiate(moonball, spawnPosition, Quaternion.AngleAxis(shootAngle, Vector3.forward));

        newMoonball.GetComponent<Moonball>().shootSpeed = shootSpeed;

        //Play Sound
        FindObjectOfType<AudioManager>().PlaySound("LaserShot", Random.Range(.95f, 1f));
    }

    private void ChargeLaser()
    {
        chargingLaser = true;
        currentChargeTime = chargeTime;

        newChargeCircle = Instantiate(chargeCircle, transform.position, Quaternion.identity, transform);

        //Play Sound
        FindObjectOfType<AudioManager>().PlaySound("LaserChargeShort", Random.Range(.90f, 1f));
        FindObjectOfType<AudioManager>().PlaySound("CharacterVoice-Charging", Random.Range(.95f, 1f));
    }

    private void ShootLaser()
    {
        // Spawn the laser in the direction pointing towards the mouse
        Vector2 playerToMouseDirection = PlayerToMouseDirection();
        Vector2 transformPosition = transform.position;
        Vector2 spawnPosition = transformPosition + playerToMouseDirection * shootOffset;
        float shootAngle = Vector2.SignedAngle(Vector2.right, playerToMouseDirection);
        Laser shotLaser = Instantiate(laser, spawnPosition, Quaternion.AngleAxis(shootAngle, Vector3.forward));
        shotLaser.shootTime = shootTime;

        currentShootTime = shootTime;
        shootingLaser = true;

        Destroy(newChargeCircle);
        chargingLaser = false;
        laserCooldownTime = laserCooldown;

        //Play Audio
        FindObjectOfType<AudioManager>().PlaySound("LaserChargeFire", Random.Range(.95f, 1f));
        //Stop Sound
        FindObjectOfType<AudioManager>().Stop("LaserChargeShort");
    }

    public void Reset()
    {
        Destroy(newChargeCircle);
        chargingLaser = false;
        currentChargeTime = 0;
        shootingLaser = false;
        currentShootTime = 0;
        laserCooldownTime = 0;
    }

    private void Reload()
    {
        Instantiate(myParticles, newMoonball.transform.position, Quaternion.identity);
        //Play Audio
        FindObjectOfType<AudioManager>().PlaySound("WeaponReload", Random.Range(.95f, 1f));

        Destroy(newMoonball);
        reloading = true;
        StartCoroutine(ReloadDelay());
    }

    private IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(reloadTime);
        //Play Audio
        FindObjectOfType<AudioManager>().PlaySound("WeaponReloadEnd", Random.Range(1.15f, 1.25f));
        //Stop Sound
        FindObjectOfType<AudioManager>().Stop("WeaponReload");
        reloading = false;
    }
}
