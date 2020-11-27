using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float shootSpeed = 25f;
    [SerializeField] private float shootOffset = 1f;  // Specifies how far from the player the projectile spawns
    [SerializeField] private Moonball moonball;

    private Moonball newMoonball;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !GameManager.playerIsDead && newMoonball == null)
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

        Vector2 transformPosition = transform.position;
        Vector2 spawnPosition = transformPosition + playerToMouse.direction * shootOffset;
        float shootAngle = Vector2.SignedAngle(Vector2.right, playerToMouse.direction);
        newMoonball = Instantiate(moonball, spawnPosition, Quaternion.AngleAxis(shootAngle, Vector3.forward));

        newMoonball.shootSpeed = shootSpeed;
    }
}
