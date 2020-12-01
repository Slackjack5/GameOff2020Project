using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float shootTime { get; set; }

    protected LineRenderer lineRenderer;
    protected RaycastHit2D firstLaserHit;
    protected EdgeCollider2D edgeCollider;
    protected List<Vector2> edgeColliderPoints;
    private float currentShootTime;

    protected virtual void Start()
    {
        currentShootTime = shootTime;
        lineRenderer = GetComponent<LineRenderer>();

        firstLaserHit = LaserHit(transform.position, transform.right, false);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, firstLaserHit.point);

        // Create an EdgeCollider that runs along the path of the laser
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.isTrigger = true;

        edgeColliderPoints = new List<Vector2>();

        // Use InverseTransformPoint since the points of the edge collider are relative to the transform of the laser object
        edgeColliderPoints.Add(edgeCollider.transform.InverseTransformPoint(transform.position));
        edgeColliderPoints.Add(edgeCollider.transform.InverseTransformPoint(firstLaserHit.point));
        edgeCollider.points = edgeColliderPoints.ToArray();
    }

    protected virtual void FixedUpdate()
    {
        // Make the laser thinner over time
        lineRenderer.startWidth -= (1 / shootTime) * Time.fixedDeltaTime;

        currentShootTime -= Time.fixedDeltaTime;
        if (currentShootTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected RaycastHit2D LaserHit(Vector2 from, Vector2 direction, bool bounce)
    {
        // Find the point where the laser will collide
        RaycastHit2D hit = new RaycastHit2D();
        RaycastHit2D[] results = Physics2D.RaycastAll(from, direction);
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].collider.gameObject.tag != "Weapon" &&
                results[i].collider.gameObject.tag != "EnemyWeapon" &&
                results[i].collider.gameObject.tag != "Enemy" && 
                results[i].collider.gameObject.tag != "Player")
            {
                // Ignore the raycast result if the laser is bouncing and the first laser hit point is the same as the result
                if (!bounce || bounce && results[i].point != firstLaserHit.point)
                {
                    return results[i];
                }
            }
        }

        return hit;
    }
}