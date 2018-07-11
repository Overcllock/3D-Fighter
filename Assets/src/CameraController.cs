using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace game
{
	public class CameraController : MonoBehaviour 
	{
		Camera cam;
		CharacterController cctl;

		public float mouseSensitivity = 1.1f;

		public Transform target;
		public float dstFromTarget = 2;
		public Vector2 pitchMinMax = new Vector2(-40, 85);

		public float rotationSmoothTime = .12f;
		Vector3 rotationSmoothVelocity;
		Vector3 currentRotation;

		float yaw;
		float pitch;

		void Start () 
		{
			cam = Camera.main;
			cctl = gameObject.GetComponent<CharacterController>();
			//m_look = new MouseLook();
			//m_look.Init(cctl.transform, cam.transform);
		}
		
		void LateUpdate () 
		{
			//m_look.LookRotation(cctl.transform, cam.transform);

			yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
			pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
			pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

			currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
			transform.eulerAngles = currentRotation;

			transform.position = target.position - transform.forward * dstFromTarget;
		}
	}
}
