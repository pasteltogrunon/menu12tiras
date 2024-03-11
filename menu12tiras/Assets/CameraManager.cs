using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineVirtualCamera menuCamera;
    public CinemachineVirtualCamera playerCamera;
    public Animator playerCameraAnimator;
    
    [Header("Camera Dynamic Settings")]
    [Range(80, 105)] public float fov;
    [Range(-0.4f, 0.4f)] public float followOffsetY;
    [Range(-0.15f, 0.15f)] public float trackedObjectOffsetY;

    [Header("Parameters")]
    public Player player;

    public void Start()
    {
        fov = 80;
        followOffsetY = 0.4f;
        trackedObjectOffsetY = 0.15f;
    }

    void Update()
    {
        playerCamera.m_Lens.FieldOfView = Mathf.Clamp(80 + (player.USpeed - 2f) * 1.5625f, 80, 105);

        playerCameraAnimator.SetBool("PlayerInverted", player.Inverted);

        playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, followOffsetY, -0.4f);
        playerCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(0, trackedObjectOffsetY, 0.5f);
    }
}
