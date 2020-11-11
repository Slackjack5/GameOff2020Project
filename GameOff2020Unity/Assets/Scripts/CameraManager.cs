using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera parabolaVcam;
    [SerializeField] private CinemachineVirtualCamera dollyVcam;
    [SerializeField] private bool useDolly = false;
    [SerializeField] private float cameraMoveSpeed = 0.5f;
    [SerializeField] private float zoomStrength = 0.06f;  // Represents how far the parabola camera zooms out
    [SerializeField] private List<Transform> roomTransforms;

    private CinemachineTrackedDolly dolly;

    private bool isExiting = false;
    private int targetRoomIndex = 0;

    // These values are used to compute the camera's position along a parabola
    private float startPositionX = 0f;
    private float targetPositionX = 0f;
    private float currentPositionX = 0f;
    private float startPositionZ;

    private void Awake()
    {
        dolly = dollyVcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        startPositionZ = parabolaVcam.transform.position.z;

        if (useDolly)
        {
            parabolaVcam.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (isExiting)
        {
            if (useDolly)
            {
                int targetPathPosition = 3 * targetRoomIndex;

                if (dolly.m_PathPosition < targetPathPosition)
                {
                    dolly.m_PathPosition += (cameraMoveSpeed * 0.1f);
                }
                else
                {
                    dolly.m_PathPosition = targetPathPosition;
                    isExiting = false;
                }
            }
            else
            {
                if (currentPositionX < targetPositionX)
                {
                    currentPositionX += cameraMoveSpeed;
                    parabolaVcam.transform.position = new Vector3(currentPositionX, parabolaVcam.transform.position.y, ComputePositionZ());
                }
                else
                {
                    parabolaVcam.transform.position = new Vector3(targetPositionX, parabolaVcam.transform.position.y, startPositionZ);
                    startPositionX = targetPositionX;
                    currentPositionX = targetPositionX;
                    isExiting = false;
                }
            }
        }
    }

    public void ExitRoom()
    {
        if (targetRoomIndex < roomTransforms.Count - 1)
        {
            isExiting = true;
            targetRoomIndex++;

            startPositionX = parabolaVcam.transform.position.x;
            targetPositionX = roomTransforms[targetRoomIndex].position.x;
        }
    }

    // Uses equation of the parabola to compute the camera's Z position depending on its X position
    private float ComputePositionZ()
    {
        return startPositionZ + zoomStrength * (currentPositionX - startPositionX) * (currentPositionX - targetPositionX);
    }
}
