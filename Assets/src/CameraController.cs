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
		MouseLook m_look;

		void Start () 
		{
			cam = Camera.main;
			cctl = gameObject.GetComponent<CharacterController>();
			m_look = new MouseLook();
			m_look.Init(cctl.transform, cam.transform);
		}
		
		void LateUpdate () 
		{
			m_look.LookRotation(cctl.transform, cam.transform);
		}
	}
}
