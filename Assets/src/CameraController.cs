using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace game
{
	public class CameraController : MonoBehaviour 
	{
		Camera cam;
		MouseLook m_look;
		CharacterController cctl;

		void Start () 
		{
			cam = gameObject.GetChild("camera").GetComponent<Camera>();
			cctl = gameObject.GetComponent<CharacterController>();
			m_look = new MouseLook();
			m_look.Init(cctl.transform, cam.transform);
		}
		
		void FixedUpdate () 
		{
			m_look.LookRotation(cctl.transform, cam.transform);
		}
	}
}
