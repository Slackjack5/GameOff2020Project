using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private float shootSpeed;
    private bool hit = false;

    public void SetShootSpeed(float shootSpeed)
    {
        this.shootSpeed = shootSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 moveDistance = transform.right * shootSpeed * Time.fixedDeltaTime;

        if (hit)
        {
            // Shrink the laser as it gets absorbed into the thing it collided with
            transform.localScale = new Vector2(transform.localScale.x - shootSpeed * Time.fixedDeltaTime, transform.localScale.y);
            transform.position += moveDistance / 2;

            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position += moveDistance;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
    }
}
