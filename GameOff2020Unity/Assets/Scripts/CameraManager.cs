using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera dollyVcam;
    [SerializeField] private CinemachineSmoothPath dollyTrack;
    [SerializeField] private float cameraMoveSpeed = 0.05f;
    [SerializeField] private float zoomOutDistance = 40f;
    [SerializeField] private List<Transform> roomTransforms;
    [SerializeField] private float leftWaypointOffset = 9f;   // Represents how far left the waypoint preceding the waypoint of the target room should be
    [SerializeField] private float rightWaypointOffset = 6f;  // Represents how far right the waypoint following the waypoint of the exited room should be

    private CinemachineTrackedDolly dolly;
    private float initialPositionZ;

    private bool isExiting = false;
    private int targetRoomIndex = 0;

    private void Awake()
    {
        dolly = dollyVcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        initialPositionZ = dollyVcam.transform.position.z;
        SetWaypoints();
    }

    private void SetWaypoints()
    {
        List<CinemachineSmoothPath.Waypoint> waypoints = new List<CinemachineSmoothPath.Waypoint>();
        for (int i = 0; i < roomTransforms.Count; i++)
        {
            float currentRoomPositionX = roomTransforms[i].position.x;

            // Each room except the first room has a waypoint to the left of it
            if (i > 0)
            {
                CinemachineSmoothPath.Waypoint leftWaypoint = new CinemachineSmoothPath.Waypoint
                {
                    position = new Vector3(currentRoomPositionX - leftWaypointOffset, dollyVcam.transform.position.y, initialPositionZ - zoomOutDistance)
                };
                waypoints.Add(leftWaypoint);
            }

            // Every room has a waypoint at its center
            CinemachineSmoothPath.Waypoint centerWaypoint = new CinemachineSmoothPath.Waypoint
            {
                position = new Vector3(currentRoomPositionX, dollyVcam.transform.position.y, initialPositionZ)
            };
            waypoints.Add(centerWaypoint);

            // Each room except the last room has a waypoint to the right of it
            if (i < roomTransforms.Count - 1)
            {
                CinemachineSmoothPath.Waypoint rightWaypoint = new CinemachineSmoothPath.Waypoint
                {
                    position = new Vector3(currentRoomPositionX + rightWaypointOffset, dollyVcam.transform.position.y, initialPositionZ - zoomOutDistance)
                };
                waypoints.Add(rightWaypoint);
            }
        }

        dollyTrack.m_Waypoints = waypoints.ToArray();
    }

    private void FixedUpdate()
    {
        if (isExiting)
        {
            // There are 3 waypoints to get from one room to the next
            int targetPathPosition = 3 * targetRoomIndex;

            if (dolly.m_PathPosition < targetPathPosition)
            {
                dolly.m_PathPosition += cameraMoveSpeed;
            }
            else
            {
                dolly.m_PathPosition = targetPathPosition;
                isExiting = false;
            }
        }
    }

    public void ExitRoom()
    {
        if (targetRoomIndex < roomTransforms.Count - 1)
        {
            isExiting = true;
            targetRoomIndex++;
        }
    }
}
