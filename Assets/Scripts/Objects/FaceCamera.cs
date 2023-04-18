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

	private void Update()
    {
        CalculateAndFaceCamera();
    }

	private void CalculateAndFaceCamera()
	{
		transform.LookAt(cam.transform.position);
	}
}
