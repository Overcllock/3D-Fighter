using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class Character : MonoBehaviour
	{
		public bool is_moving = false;
		public bool is_spawned = false;

		vThirdPersonCamera cam;

		void Start()
		{
			cam = Camera.main.gameObject.GetComponent<vThirdPersonCamera>();
		}

		public void OnSpawned()
		{
			is_spawned = true;
			cam.SetMainTarget(transform);
			cam.lockCamera = false;
		}

		public static Character Load(Account data)
		{
			//TODO: load character
			return new Character();
		}
	}
}
