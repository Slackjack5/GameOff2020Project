﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPivot : MonoBehaviour
{
    public void Pivot()
    {
        // Compute the direction from the player to the mouse
        float distanceFromCameraToWorld = transform.position.z - Camera.main.transform.position.z;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCameraToWorld));
        Ray2D playerToMouse = new Ray2D(transform.position, mousePosition - transform.position);
        Vector2 playerToMouseDirection = playerToMouse.direction;

        float rotationZ = Mathf.Atan2(playerToMouseDirection.y, playerToMouseDirection.x) * Mathf.Rad2Deg;

        // Rotate the arm towards the mouse
        // Turn the arm rightside up when the arm is on the opposite side of the direction the player is facing
        if (rotationZ < -90 || rotationZ > 90)
        {
            transform.rotation = Quaternion.Euler(180, 0, -rotationZ);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        }
    }

    public void deactivate()
    {
        gameObject.SetActive(false);
    }

    public void activate()
    {
        gameObject.SetActive(true);
    }
}
