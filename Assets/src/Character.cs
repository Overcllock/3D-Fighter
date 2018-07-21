﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace game
{
	public class Character : MonoBehaviour
	{
		const float SKILLS_STORING_INTERVAL = 0.75f;
		
		float queue_duration = 0;
		public bool is_moving = false;
		public bool is_spawned = false;
		public bool is_use_ability
		{
			get { return active_ability != null; }
		}

		public EnumControl control = EnumControl.NONE;

		vThirdPersonCamera cam;
		Animator animator = null;
		public MovementController mctl = null;
		public HUD hud = null;
		public List<Ability> abilites = null;
		public Ability active_ability = null;
		Queue<Ability> skills_queue = null;
		Coroutine aab_defer_routine = null;

		void Awake()
		{
			skills_queue = new Queue<Ability>();
		}

		void Start()
		{
			animator = GetComponent<Animator>();
			mctl = GetComponent<MovementController>();
			Init();
			InitAbilites();
		}

		void Update()
		{
			ProcessInput();
			TickAbilites();
			UpdateSkillsQueue();
		}

		void Init()
		{
			cam = Camera.main.gameObject.GetComponent<vThirdPersonCamera>();
		}

		void InitAbilites()
		{
			abilites = new List<Ability>();
			for(int i = 0; i < Main.self.all_abilites_keys.Length; ++i)
			{
				var key = Main.self.all_abilites_keys[i];
				var ability = Ability.GetByKey(key);
				if(ability != null)
					abilites.Add(ability);
			}
		}

		void TickAbilites()
		{
			for(int i = 0; i < abilites.Count; ++i)
			{
				var ab = abilites[i];
				ab.TickCooldown();
				ab.Tick(Time.deltaTime);
				if(hud != null)
					hud.UpdateCooldown(ab.key, ab.cooldown_percent);
			}
		}

		void UpdateSkillsQueue()
		{
			if(skills_queue.Count == 0)
				return;

			queue_duration += Time.deltaTime;
			if(queue_duration >= SKILLS_STORING_INTERVAL)
			{
				skills_queue.Dequeue();
				queue_duration = 0;
				return;
			}

			var skill = skills_queue.Dequeue();
			if(!skill.TryUseAbility())
				skills_queue.Enqueue(skill);
		}

		void ProcessInput()
		{
			//Keyboard
			for(int i = 0; i < Main.self.all_keys.Length; ++i)
			{
				var key = Main.self.all_keys[i];
				bool is_held = Input.GetKey(key);
				if(is_held)
				{
					var ab = abilites.FindByKey(key);
					if(ab != null && !is_use_ability)
					{
						ab.inflictor = this;
						if(!ab.TryUseAbility())
							skills_queue.Enqueue(ab);
					}
				}
				if(hud != null)
					hud.PushSkill((EnumAbilitesKeys)key, is_held);
			}

			//Mouse
			for(int i = (int)EnumAbilitesKeys.KEY_LMB_1; i <= (int)EnumAbilitesKeys.KEY_RMB; ++i)
			{
				bool is_held = Input.GetMouseButton(i);
				bool is_pressed = Input.GetMouseButtonDown(i);
				Ability ab = null;
				if(is_pressed)
				{
					ab = abilites.FindByKey((EnumAbilitesKeys)i);
					if(i == (int)EnumAbilitesKeys.KEY_LMB_1 && active_ability == ab)
						ab = abilites.FindByKey(EnumAbilitesKeys.KEY_LMB_2);
					if(ab != null)
					{
						ab.inflictor = this;
						if(!ab.TryUseAbility())
							skills_queue.Enqueue(ab);
					}
				}
				if(hud != null)
					hud.PushSkill((EnumAbilitesKeys)i, is_held);
			}
		}

		public IEnumerator WaitAndDo(UnityAction func, float delay)
		{
			yield return new WaitForSecondsRealtime(delay);
			func();
		}

		public IEnumerator MoveForward(float delay)
		{
			float dt = 0;
			while(dt < delay)
			{
				mctl.MoveForward(6.0f);
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
			{
				if(aab_defer_routine != null)
				{
					StopCoroutine(aab_defer_routine);
					aab_defer_routine = null;
				}
				animator.Play(statename, 0);
			}
			
			aab_defer_routine = StartCoroutine(AnimDefer(delay));
		}

		IEnumerator AnimDefer(float delay = 0.0f)
		{
			yield return new WaitForSecondsRealtime(delay);
			if(active_ability != null)
			{
				active_ability.Defer();
				active_ability = null;
			}
			aab_defer_routine = null;
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
