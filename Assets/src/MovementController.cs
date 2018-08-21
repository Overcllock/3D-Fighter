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

		//TODO: move to input settings
		KeyCode[] ALL_KEYS = new KeyCode[] {
			KeyCode.W,
			KeyCode.A,
			KeyCode.S,
			KeyCode.D
		};
		Stack<KeyCode> last_input;

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
			last_input = new Stack<KeyCode>(ALL_KEYS.Length);
			cctl = gameObject.GetComponent<CharacterController>();
			animator = gameObject.GetComponent<Animator>();
			cam = Camera.main.gameObject.GetComponent<vThirdPersonCamera>();
		}

		void Update()
		{
			if(last_input.Count > 0)
			{
				if(!Input.GetKey(last_input.Peek()))
					last_input.Pop();
			}

			var last_input_key = GetLastInput();
			if(last_input_key != KeyCode.Break)
				last_input.Push(last_input_key);
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

			current_speed = new Vector2(cctl.velocity.x, cctl.velocity.z).magnitude;
		}

		void MoveCamera(Vector2 mouse_input, bool follow_character_transform = false)
		{
			var directory = GetDirectory();

			if(follow_character_transform)
				cam.AutoRotateCamera(x: true, y: false);
			else
				cam.RotateCamera(mouse_input.x, mouse_input.y);

			if(moving_allowed)
				RotateWithAnotherTransform(cam.transform, directory.ToOffset(transform));
		}

		void RotateWithAnotherTransform(Transform referenceTransform, float angleOffset = 0.0f)
		{
			var newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y + angleOffset, transform.eulerAngles.z);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), STRAFE_ROTATION_SPEED * Time.fixedDeltaTime);
		}

		Vector3 GetDirectory()
		{
			if(last_input.Count == 0) 
				return Vector3.zero;

			var directory = Vector3.zero;
			switch(last_input.Peek())
			{
				case KeyCode.W:
					directory = transform.forward;
					break;
				case KeyCode.S:
					directory = -transform.forward;
					break;
				case KeyCode.A:
					directory = -transform.right;
					break;
				case KeyCode.D:
					directory = transform.right;
					break;
			}

			return directory;
		}

		KeyCode GetLastInput()
		{
			KeyCode input = KeyCode.Break;
			for(int i = 0; i < ALL_KEYS.Length; ++i)
			{
				var key = ALL_KEYS[i];
				if(Input.GetKeyDown(key))
				{
					input = key;
					break;
				}
			}
			return input;
		}
	}
}
