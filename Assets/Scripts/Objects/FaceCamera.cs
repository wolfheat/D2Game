using Cinemachine;
using System;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
	Camera cam;

	private void Awake()
    {
        cam = Camera.main;
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerController>() != null)
		{
			Debug.Log("PICK ME UP");
		}
	}

	private void Update()
    {
        CalculateAndFaceCamera();
    }

	private void CalculateAndFaceCamera()
	{
		transform.LookAt(cam.transform.position);
	}
}
