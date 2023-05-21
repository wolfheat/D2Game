using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotateController : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera vcam;
	CinemachineTransposer vcamTransposer;
	CinemachineComponentBase vcamComponentBase;

	private Camera mainCamera;

    private const float RotationSpeed = 120f;
    private const float RotationSpeedAcceleration = 1000f;
    private const float Damping = 0.87f;
	
	private float rotationSpeed = 0f;
	
	private const float ZoomlevelTime = 0.28f;
    private const int ZoomLevelMinimumHeight = 5;
    private const int ZoomLevelStepHeight = 2;

    [Range (0,8)] private int zoomLevel = 6;
	private float cameraHeight = 15;


	private Vector3 zoomLevelStart;
	private Vector3 zoomLevelEnd;
	private float zoomlevelTimer = 0;
    private bool zooming = false;

	private Vector3 followOffset = new Vector3(0f, 15f, -12f);

	
	
	private void OnEnable()
	{
		mainCamera = Camera.main;		
		vcamTransposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
		vcamComponentBase = vcam.GetCinemachineComponent<CinemachineComponentBase>();
		vcamTransposer.m_FollowOffset = followOffset;
		SetZoom();
		// Subscribe to input
		Inputs.Instance.Controls.Land.ZoomIn.performed += _ => ZoomCamera();
	}
		
	private void Update()
    {
		CheckPlayerInput();

        if (rotationSpeed != 0) RotateBySpeed();
        if (zooming) ZoomLerp();		
	}

    private void FixedUpdate()
    {
        rotationSpeed *= Damping;
    }

    private void ZoomLerp()
    {
        (vcamComponentBase as CinemachineTransposer).m_FollowOffset = Vector3.Lerp(zoomLevelStart, zoomLevelEnd, zoomlevelTimer / ZoomlevelTime);
		zoomlevelTimer += Time.deltaTime;
		if (zoomlevelTimer >= ZoomlevelTime)
		{
			(vcamComponentBase as CinemachineTransposer).m_FollowOffset = zoomLevelEnd;
			zooming = false;
			zoomlevelTimer = 0;
        }
    }

    private void RotateBySpeed()
    {
        vcamTransposer.m_FollowOffset = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up) * vcamTransposer.m_FollowOffset;
		//Debug.Log("frams per second: "+(1000/Time.deltaTime));
    }

    private void CheckPlayerInput()
	{
		// Constantly checks for inputs
		if (Inputs.Instance.Controls.Land.RotateL.ReadValue<float>() == 1) RotateCamera();
		if (Inputs.Instance.Controls.Land.RotateR.ReadValue<float>() == 1) RotateCamera(-1);

	}

	private void ZoomCamera()
	{
		float scroll = Mouse.current.scroll.ReadValue().normalized.y;
		zoomLevel = Mathf.Clamp(zoomLevel+(int)scroll,1,8);
		SetZoom();	
	}

	private void SetZoom()
	{
		if (!zooming)
			zoomLevelStart = (vcamComponentBase as CinemachineTransposer).m_FollowOffset;

		cameraHeight = ZoomLevelMinimumHeight + zoomLevel * ZoomLevelStepHeight;
		zoomLevelEnd = new Vector3(zoomLevelStart.x, cameraHeight, zoomLevelStart.z);

        zooming = true;
    }


	private void RotateCamera(int dirMultiplier = 1)
	{
		// rotation speed changed and clamped to range
		rotationSpeed = Mathf.Clamp(rotationSpeed + dirMultiplier * RotationSpeedAcceleration*Time.deltaTime, -RotationSpeed, RotationSpeed);
    }
}
