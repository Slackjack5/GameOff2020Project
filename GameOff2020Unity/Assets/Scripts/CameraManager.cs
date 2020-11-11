using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera dollyVcam;
    [SerializeField] private float cameraMoveSpeed = 0.05f;
    [SerializeField] private List<Transform> roomTransforms;

    private CinemachineTrackedDolly dolly;

    private bool isExiting = false;
    private int targetRoomIndex = 0;

    private void Awake()
    {
        dolly = dollyVcam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    private void FixedUpdate()
    {
        if (isExiting)
        {
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
