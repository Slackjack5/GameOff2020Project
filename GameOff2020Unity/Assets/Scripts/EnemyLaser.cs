using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector2 laserHitPoint;
    private float shootTime;
    private float currentShootTime;

    public void SetShootTime(float shootTime)
    {
        this.shootTime = shootTime;
    }

    private void Start()
    {
        currentShootTime = shootTime;
        lineRenderer = GetComponent<LineRenderer>();

        // Find the point where the laser will collide
        RaycastHit2D[] results = Physics2D.RaycastAll(transform.position, transform.right);
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].collider.gameObject != gameObject)
            {
                laserHitPoint = results[i].point;
            }
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, laserHitPoint);
    }

    private void FixedUpdate()
    {
        // Make the laser thinner over time
        lineRenderer.startWidth -= (1 / shootTime) * Time.fixedDeltaTime;

        currentShootTime -= Time.fixedDeltaTime;
        if (currentShootTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}