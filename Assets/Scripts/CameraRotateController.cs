using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotateController : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera vcam;
	CinemachineTransposer vcamTransposer;

	private Camera mainCamera;

	private const float RotationSpeed = 120f;
	private Vector3 m_followOffset = new Vector3(0f, 15f, -12f);

	private void OnEnable()
	{
		mainCamera = Camera.main;		
		vcamTransposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
		vcamTransposer.m_FollowOffset = m_followOffset;
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

	private void RotateCamera(int dirMultiplier = 1)
	{
		Vector3 currentCameraPositionVector = vcamTransposer.m_FollowOffset;
		Vector3 newAimVector = Quaternion.AngleAxis(dirMultiplier*RotationSpeed * Time.deltaTime, Vector3.up) * currentCameraPositionVector;
		vcamTransposer.m_FollowOffset = newAimVector;
	}
}
