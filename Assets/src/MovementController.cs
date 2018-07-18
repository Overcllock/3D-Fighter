using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(Animator))]
	public class MovementController : MonoBehaviour 
	{
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
		public float speed_smooth_time = 0.1f;
		public float strafe_rotation_speed = 10f;
		public bool moving_allowed = true;

		float current_speed;
		float speed_smooth_velocity;

		void Start() 
		{	
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
			if(moving_allowed)
				Move();

			Main.self.player.is_moving = cctl.velocity.magnitude > 0.6f;
			animator.SetBool("Run", Main.self.player.is_moving && Main.self.player.active_ability == null);
		}

		void Move()
		{
			var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			var mouse_input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			var input_directory = input.normalized;
			var directory = GetDirectory();
				
			float target_speed = speed * input_directory.magnitude;
			current_speed = Mathf.SmoothDamp(current_speed, target_speed, ref speed_smooth_velocity, speed_smooth_time);

			var velocity = Vector3.ClampMagnitude(transform.forward * current_speed, target_speed);
			cctl.Move(velocity * Time.fixedDeltaTime);

			cam.RotateCamera(mouse_input.x, mouse_input.y);
			RotateWithAnotherTransform(cam.transform, directory.ToOffset(transform));

			current_speed = new Vector2(cctl.velocity.x, cctl.velocity.z).magnitude;
		}

		void RotateWithAnotherTransform(Transform referenceTransform, float angleOffset = 0.0f)
        {
            var newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y + angleOffset, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), strafe_rotation_speed * Time.fixedDeltaTime);
        }

		public void MoveForward(float speed)
		{
			var velocity = transform.forward * speed;
			cctl.Move(velocity * Time.deltaTime);
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
