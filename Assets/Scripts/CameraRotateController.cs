using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotateController : MonoBehaviour
{

	PlayerControls playerControls;
	InputAction InputActionLeft;
	InputAction InputActionRight;
	[SerializeField] CinemachineVirtualCamera vcam;
	CinemachineTransposer vcamTransposer;

	private const float RotationSpeed = 120f;

	private float cameraRotation = 0f;
	private float cameraXDistance = 0f;
	private float cameraYDistance = 15f;
	private float cameraZDistance = -12f;

	private void Initiate()
    {
		InputActionLeft = playerControls.Land.RotateL;
		InputActionRight = playerControls.Land.RotateR;
		vcamTransposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
		vcamTransposer.m_FollowOffset = new Vector3(cameraXDistance,cameraYDistance,cameraZDistance);
	}

	public void SetPlayerControls(PlayerControls p)
    {
		playerControls = p;
		Initiate();
	}
	
	private void Update()
    {
		CheckPlayerInput();

	}

	private void CheckPlayerInput()
	{
		if (InputActionLeft.ReadValue<float>() == 1) RotateCameraLeft();
		if (InputActionRight.ReadValue<float>() == 1) RotateCameraRight();
	}

	private void RotateCameraLeft(int dirMultiplier = 1)
	{

		Vector3 currentCameraPositionVector = vcamTransposer.m_FollowOffset;
		/* maybe not needed to flatten
		Vector3 flatAimVector = currentCameraPositionVector;
		float aimVectorY = currentCameraPositionVector.y;
		flatAimVector.y = 0f;
		*/

		Vector3 newAimVector = Quaternion.AngleAxis(dirMultiplier*RotationSpeed * Time.deltaTime, Vector3.up) * currentCameraPositionVector;
		vcamTransposer.m_FollowOffset = newAimVector;

		//Debug.Log("Rotating left By: "+ RotationSpeed);
	}

	private void RotateCameraRight()
	{
		RotateCameraLeft(-1);
	}
}
