using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class Character : MonoBehaviour
	{
		public bool is_moving = false;
		public bool is_spawned = false;
		public bool is_use_ability = false;
		public EnumControl control = EnumControl.NONE;

		vThirdPersonCamera cam;
		Animator animator = null;
		public MovementController mctl = null;
		public HUD hud = null;
		public List<Ability> abilites = null;
		public Ability active_ability = null;

		void Start()
		{
			animator = GetComponent<Animator>();
			mctl = GetComponent<MovementController>();
			Init();
			InitAbilites();
			StartCoroutine(TickAbilites());
		}

		void Update()
		{
			ProcessInput();
		}

		void Init()
		{
			cam = Camera.main.gameObject.GetComponent<vThirdPersonCamera>();
		}

		void InitAbilites()
		{
			abilites = new List<Ability>();
			for(int i = 0; i < Main.self.all_keys.Length; ++i)
			{
				var key = Main.self.all_abilites_keys[i];
				var ability = Ability.GetByKey(key);
				if(ability != null)
					abilites.Add(ability);
			}
		}

		IEnumerator TickAbilites()
		{
			while(true)
			{
				yield return new WaitForSecondsRealtime(1.0f);
				for(int i = 0; i < abilites.Count; ++i)
				{
					var ab = abilites[i];
					ab.TickCooldown();
				}
				//TODO:
			}
		}

		void ProcessInput()
		{
			//Keyboard
			for(int i = 0; i < Main.self.all_keys.Length; ++i)
			{
				var key = Main.self.all_keys[i];
				if(Input.GetKey(key))
				{
					var ab = abilites.FindByKey(key);
					if(ab != null && !is_use_ability)
					{
						ab.inflictor = this;
						ab.TryUseAbility();
					}
				}
			}

			//Mouse
			for(int i = (int)EnumAbilitesKeys.KEY_LMB_1; i <= (int)EnumAbilitesKeys.KEY_RMB; ++i)
			{
				if(Input.GetMouseButtonDown(i))
				{
					var ab = abilites.FindByKey((EnumAbilitesKeys)i);
					if(ab != null && !is_use_ability)
					{
						if(i == (int)EnumAbilitesKeys.KEY_LMB_1 && active_ability == ab)
						{
							ab = abilites.FindByKey(EnumAbilitesKeys.KEY_LMB_2);
							if(ab == null)
								return;
						}
						ab.inflictor = this;
						ab.TryUseAbility();
					}
				}
			}
		}

		public IEnumerator MoveForward(float delay)
		{
			float dt = 0;
			while(dt < delay)
			{
				mctl.MoveForward(5.0f);
				dt += Time.deltaTime;
				yield return new WaitForFixedUpdate();
			}
			mctl.moving_allowed = true;
		}

		public void Spawn()
		{
			transform.position = GameObject.Find("startpoint_1").transform.position;
			gameObject.SetActive(true);
			OnSpawned();
		}

		public void PlayAnim(string statename, bool is_animlock, float delay)
		{
			if(is_animlock)
				animator.SetBool(statename, true);
			else
				animator.Play(statename, 0);
			
			StartCoroutine(ResetActiveAbility(delay));
		}

		IEnumerator ResetActiveAbility(float delay = 0.0f)
		{
			yield return new WaitForSecondsRealtime(delay);
			active_ability = null;
		}

		void OnSpawned()
		{
			Init();
			is_spawned = true;
			cam.SetMainTarget(transform);
			cam.lockCamera = false;
			cam.cutsceneMode = false;
			GameObject.Find("bird").SetActive(false);
		}
	}
}
