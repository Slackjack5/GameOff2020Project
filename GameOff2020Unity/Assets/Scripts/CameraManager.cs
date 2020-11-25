using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera dollyVcam;
    [SerializeField] private CinemachineSmoothPath dollyTrack;
    [SerializeField] private float cameraMoveSpeed = 0.25f;
    [SerializeField] private List<Transform> roomTransforms;

    private Camera camera;
    private CinemachineTrackedDolly dolly;
    private float initialPositionZ;

    private bool isExiting = false;
    private int targetRoomIndex = 0;

    const float aspect = 16f / 9f;

    private void Awake()
    {
        camera = Camera.main;
        camera.aspect = aspect;

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

            // Every room has a waypoint at its center
            CinemachineSmoothPath.Waypoint centerWaypoint = new CinemachineSmoothPath.Waypoint
            {
                position = new Vector3(currentRoomPositionX, dollyVcam.transform.position.y, initialPositionZ)
            };
            waypoints.Add(centerWaypoint);
        }

        dollyTrack.m_Waypoints = waypoints.ToArray();
    }

    private void FixedUpdate()
    {
        if (isExiting)
        {
            if (dolly.m_PathPosition < targetRoomIndex)
            {
                dolly.m_PathPosition += cameraMoveSpeed;
            }
            else
            {
                dolly.m_PathPosition = targetRoomIndex;
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
