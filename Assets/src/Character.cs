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
			Init();
		}

		void Init()
		{
			cam = Camera.main.gameObject.GetComponent<vThirdPersonCamera>();
		}

		public void Spawn()
		{
			transform.position = GameObject.Find("startpoint_1").transform.position;
			gameObject.SetActive(true);
			Init();
			OnSpawned();
		}

		void OnSpawned()
		{
			is_spawned = true;
			cam.SetMainTarget(transform);
			cam.lockCamera = false;
			cam.cutsceneMode = false;
			GameObject.Find("bird").SetActive(false);
		}

		public static Character Load(Account data)
		{
			//TODO: load character
			return new Character();
		}
	}
}
