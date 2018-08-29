using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(Animator))]
	public class MovementController : MonoBehaviour 
	{
		const float MIN_VELOCITY_MAGNITUDE = 0.6f;
		const float SPEED_SMOOTH_TIME = 0.1f;
		const float STRAFE_ROTATION_SPEED = 10f;

		CharacterController cctl;
		Animator animator;
		vThirdPersonCamera cam;

		public float speed = 5.0f;
		public bool moving_allowed;
		public bool keep_camera_look_at;

		float current_speed;
		float speed_smooth_velocity;

		void Awake()
		{
			keep_camera_look_at = false;
			cctl = gameObject.GetComponent<CharacterController>();
			animator = gameObject.GetComponent<Animator>();
			cam = Camera.main.gameObject.GetComponent<vThirdPersonCamera>();
		}
		
		void FixedUpdate() 
		{
			Move();
			Main.self.player.is_moving = cctl.velocity.magnitude > MIN_VELOCITY_MAGNITUDE;
			animator.SetBool("Run", Main.self.player.is_moving && !Main.self.player.is_use_ability);
		}

		void Move()
		{
			var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			var mouse_input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

			if(moving_allowed)
				MoveCharacter(input);
			MoveCamera(mouse_input, keep_camera_look_at);
		}

		void MoveCharacter(Vector2 input)
		{
			var input_directory = input.normalized;

			float target_speed = speed * input_directory.magnitude;
			current_speed = Mathf.SmoothDamp(current_speed, target_speed, ref speed_smooth_velocity, SPEED_SMOOTH_TIME);

			var velocity = Vector3.ClampMagnitude(transform.forward * current_speed, target_speed);
			cctl.Move(velocity * Time.fixedDeltaTime);

			float offset = 0;
			if(input.y != 0 && input.x != 0)
			{
				offset = Mathf.Clamp(input.y * 180.0f, -180.0f, 0);
				float x_offset = input.x * 45.0f;
				offset += offset < 0 ? -x_offset : x_offset;
			}
			else if(input.y != 0)
				offset = Mathf.Clamp(input.y * 180.0f, -180.0f, 0);
			else if(input.x != 0)
				offset = input.x * 90.0f;
			
			RotateWithAnotherTransform(cam.transform, offset);

			current_speed = new Vector2(cctl.velocity.x, cctl.velocity.z).magnitude;
		}

		void MoveCamera(Vector2 mouse_input, bool follow_character_transform = false)
		{
			if(follow_character_transform)
				cam.AutoRotateCamera(x: true, y: false);
			else
				cam.RotateCamera(mouse_input.x, mouse_input.y);
		}

		void RotateWithAnotherTransform(Transform referenceTransform, float angleOffset = 0.0f)
		{
			var newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y + angleOffset, transform.eulerAngles.z);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), STRAFE_ROTATION_SPEED * Time.fixedDeltaTime);
		}
	}
}
