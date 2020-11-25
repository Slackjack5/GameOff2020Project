using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private CapsuleCollider2D capsuleCollider;
    private Vector3 laserHitPoint;
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

        // Create a CapsuleCollider that covers the shape of the laser
        capsuleCollider = gameObject.AddComponent<CapsuleCollider2D>();
        capsuleCollider.transform.position = transform.position + (laserHitPoint - transform.position) / 2;
        capsuleCollider.direction = CapsuleDirection2D.Horizontal;
        capsuleCollider.isTrigger = true;
        capsuleCollider.size = new Vector2((laserHitPoint - transform.position).magnitude * 2, 1);
        capsuleCollider.offset = Vector2.zero;
    }

    private void FixedUpdate()
    {
        // Make the laser thinner over time
        lineRenderer.startWidth -= (1 / shootTime) * Time.fixedDeltaTime;
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, lineRenderer.startWidth);

        currentShootTime -= Time.fixedDeltaTime;
        if (currentShootTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}