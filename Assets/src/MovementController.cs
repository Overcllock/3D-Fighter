using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(Animator))]
	public class MovementController : MonoBehaviour 
	{
		CharacterController cctl;
		Camera cam;
		Animator animator;
		vThirdPersonCamera t_cam;

		public float speed = 2.0f;
		public float speed_smooth_time = 0.1f;
		public bool moving_allowed = true;

		float current_speed;
		float speed_smooth_velocity;

		void Start() 
		{
			cctl = gameObject.GetComponent<CharacterController>();
			animator = gameObject.GetComponent<Animator>();
			cam = Camera.main;
			t_cam = cam.gameObject.GetComponent<vThirdPersonCamera>();
		}
		
		void LateUpdate() 
		{
			if(moving_allowed)
				Move();
			
			Main.self.player.is_moving = cctl.velocity != Vector3.zero;
			animator.SetBool("Run", Main.self.player.is_moving);
		}

		void Move()
		{
			var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			var mouse_input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			var input_directory = input.normalized;
			var directory = GetDirectory();
				
			float target_speed = speed * input_directory.magnitude;
			current_speed = Mathf.SmoothDamp(current_speed, target_speed, ref speed_smooth_velocity, speed_smooth_time);

			var velocity = Vector3.ClampMagnitude(directory * current_speed, speed);
			cctl.Move(velocity * Time.deltaTime);
			t_cam.RotateCamera(mouse_input.x, mouse_input.y);

			current_speed = new Vector2(cctl.velocity.x, cctl.velocity.z).magnitude;
		}

		Vector3 GetDirectory()
		{
			var directory = Vector3.zero;

			if(Input.GetKey(KeyCode.W))
				directory += transform.forward;
			if(Input.GetKey(KeyCode.A))
				directory -= transform.right;
			if(Input.GetKey(KeyCode.S))
				directory -= transform.forward;
			if(Input.GetKey(KeyCode.D))
				directory += transform.right;

			return directory;
		}
	}
}
