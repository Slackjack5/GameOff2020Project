using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : Laser
{
    protected override void Start()
    {
        base.Start();

        // Have the laser bounce against a surface
        Vector2 transformPosition = transform.position;
        Vector2 firstLaserVector = firstLaserHit.point - transformPosition;
        Vector2 reflectedVector = Vector2.Reflect(firstLaserVector, firstLaserHit.normal);
        RaycastHit2D secondLaserHit = LaserHit(firstLaserHit.point, reflectedVector.normalized);
        lineRenderer.SetPosition(2, secondLaserHit.point);

        edgeColliderPoints.Add(edgeCollider.transform.InverseTransformPoint(secondLaserHit.point));
        edgeCollider.points = edgeColliderPoints.ToArray();
    }
}
