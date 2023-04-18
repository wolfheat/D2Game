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

	[Range (1,8)]
	private int zoomLevel = 6;
	private float cameraDistance = 15;

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
		Vector3 currentOffset = (vcamComponentBase as CinemachineTransposer).m_FollowOffset;
		cameraDistance = 5 + zoomLevel * 2;
		(vcamComponentBase as CinemachineTransposer).m_FollowOffset = new Vector3(currentOffset.x, cameraDistance, currentOffset.z);
	}


	private void RotateCamera(int dirMultiplier = 1)
	{
		Vector3 currentCameraPositionVector = vcamTransposer.m_FollowOffset;
		Vector3 newAimVector = Quaternion.AngleAxis(dirMultiplier*RotationSpeed * Time.deltaTime, Vector3.up) * currentCameraPositionVector;
		vcamTransposer.m_FollowOffset = newAimVector;
	}
}
