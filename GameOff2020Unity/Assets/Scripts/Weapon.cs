using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float shootSpeed = 0.3f;
    [SerializeField] private float shootOffset = 0.8f;
    [SerializeField] private Moonball moonball;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !GameManager.playerIsDead)
        {
            ShootBall();
        }
    }

    private void ShootBall()
    {
        // Spawn the projectile in the direction pointing towards the mouse
        float distanceFromCameraToWorld = transform.position.z - Camera.main.transform.position.z;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCameraToWorld));

        Ray2D playerToMouse = new Ray2D(transform.position, mousePosition - transform.position);
        Vector3 playerToMouseDirection = playerToMouse.direction;

        Vector3 spawnPosition = transform.position + playerToMouseDirection * shootOffset;
        Instantiate(moonball, spawnPosition, Quaternion.identity);
    }
}
