﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace game
{
	public class Character : MonoBehaviour
	{
		public class Control
		{
			public EnumControl type;
			public float duration;
			public string vfx;

			public UnityAction OnStart;
			public UnityAction OnFinish;

			public Control(EnumControl type, float duration, string vfx = "", UnityAction OnStart = null, UnityAction OnFinish = null)
			{
				this.type = type;
				this.duration = duration;
				this.vfx = vfx;
				this.OnStart = OnStart;
				this.OnFinish = OnFinish;
			}
		}

		const float SKILLS_STORING_INTERVAL = 0.8f;
		const float EVADING_SPEED = 3.1f;
		const float EVADING_ANGLE = 180.0f;

		public static readonly float MAX_HP = 1500.0f;
		
		float queue_duration = 0;

		[HideInInspector]
		public bool is_moving = false;
		[HideInInspector]
		public bool is_spawned = false;
		public bool is_player = false;
		public bool is_dummy = false;
		[HideInInspector]
		public bool is_invulnerable = false;

		public bool is_use_ability
		{
			get { return active_ability != null; }
		}

		public bool has_target
		{
			get { return target != null; }
		}	

		public bool has_control
		{
			get { return control != null; }
		}

		public float distance_to_target
		{
			get 
			{ 
				if(!has_target)
					return Mathf.Infinity;
				return Vector3.Distance(transform.position, target.transform.position); 
			}
		}

		vThirdPersonCamera cam;
		Animator animator = null;
		float anim_speed;
		Coroutine aab_defer_routine = null;
		Coroutine process_control = null;
		Control control = null;
		[HideInInspector]
		public Character target = null;
		[HideInInspector]
		public MovementController mctl = null;
		[HideInInspector]
		public HUD hud = null;

		public List<Ability> abilites = null;
		public Ability active_ability = null;
		Queue<Ability> abilites_queue = null;

		float hp = MAX_HP;
		public float HP
		{
			get { return hp; }
			set
			{
				hp = Mathf.Clamp(value, 0, MAX_HP);
				if(hp == 0)
					OnDie();
			}
		}

		void Awake()
		{
			abilites_queue = new Queue<Ability>();
		}

		void Start()
		{
			animator = GetComponent<Animator>();
			mctl = GetComponent<MovementController>();
			anim_speed = animator.speed;
			InitCamera();
			InitAbilites();
		}

		void FixedUpdate()
		{
			UpdateTarget();
			TickAbilites();

			if(is_player)
			{
				ProcessInput();
				UpdateAbilitesQueue();
			}
		}

		void InitCamera()
		{
			cam = Camera.main.gameObject.GetComponent<vThirdPersonCamera>();
		}

		void InitAbilites()
		{
			abilites = new List<Ability>()
			{
				new Jab(this),
				new Kick(this),
				new Counter(this),
				new Dodge(this),
				new Escape(this),
				new Headspring(this),
				new LeftEvade(this),
				new RightEvade(this),
				new Spinkick(this),
				new Screwkick(this)
			};
		}

		void TickAbilites()
		{
			for(int i = 0; i < abilites.Count; ++i)
			{
				var ab = abilites[i];
				ab.Tick(Time.fixedDeltaTime);
				if(hud != null)
					hud.UpdateCooldown(ab.conf.axis, ab.cooldown_percent, ab.cooldown);
			}
		}

		void UpdateTarget()
		{
			bool is_target_find = false;
			var ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.6F, 0));
			var hits = Physics.RaycastAll(ray, 20.0f);

			target = null;
			for(int i = 0; i < hits.Length; ++i)
			{
				var hit = hits[i];
				var target = hit.collider.gameObject.GetComponent<Character>();
				if(target != null && target != this)
				{
					if(this.target != null)
					{
						var current_dist = Vector3.Distance(transform.position, this.target.transform.position);
						var new_dist = Vector3.Distance(transform.position, target.transform.position);
						if(new_dist < current_dist)
						{
							this.target = target;
							continue;
						}
					}
					this.target = target;
				}
			}

			is_target_find = target != null;

			if(hud != null)
			{
				hud.SetCrosshairAlpha(is_target_find);
				hud.SetEnemyBarVisibility(is_target_find);

				if(is_target_find)
					hud.UpdateEnemyBar(target);
			}
		}

		void UpdateAbilitesQueue()
		{
			if(abilites_queue.Count == 0)
			{
				queue_duration = 0;
				return;
			}

			queue_duration += Time.fixedDeltaTime;
			if(queue_duration >= SKILLS_STORING_INTERVAL)
			{
				abilites_queue.Dequeue();
				queue_duration = 0;
				if(abilites_queue.Count == 0)
					return;
			}

			var skill = abilites_queue.Dequeue();
			if(!skill.TryUseAbility())
				if(!abilites_queue.Contains(skill))
					abilites_queue.Enqueue(skill);
		}

		void ProcessInput()
		{
			//Pause
			if(Input.GetButtonDown("Cancel"))
			{
				Main.self.SetPause(!Main.self.is_paused);
				cam.SetCursorVisibility(Main.self.is_paused);
			}

			for(int i = 0; i < abilites.Count; ++i)
			{
				var ability = abilites[i];
				bool is_held = ability.conf.axis.Length > 0 && Input.GetAxis(ability.conf.axis) > 0;
				if(is_held)
				{
					if(!ability.TryUseAbility() && !abilites_queue.Contains(ability))
						abilites_queue.Enqueue(ability);
				}
				if(hud != null)
					hud.PushSkill(ability.conf.axis, is_held);
			}
		}

		public void FreezeAnim()
		{
			anim_speed = animator.speed;
			animator.speed = 0f;
		}

		public void UnfreezeAnim()
		{
			animator.speed = anim_speed;
		}

		public bool HasTargetInRadius(float radius)
		{
			return FindNearestTarget(radius) != null;
		}

		public void SetControl(Control control)
		{
			if(process_control != null)
			{
				StopCoroutine(process_control);
				process_control = null;
			}

			if(has_control)
				ResetControl();

			if(control.vfx != string.Empty)
				ShowVFX(control.vfx);
			
			if(control.OnStart != null)
				control.OnStart();

			this.control = control;
			process_control = Main.self.StartCoroutine(Main.WaitAndDo(ResetControl, control.duration));
		}

		public void ResetControl()
		{
			if(control == null)
				return;

			if(control.vfx != string.Empty)
				HideVFX(control.vfx);

			if(control.OnFinish != null)
				control.OnFinish();

			if(process_control != null)
			{
				Main.self.StopCoroutine(process_control);
				process_control = null;
			}

			control = null;
		}

		public IEnumerator Evade(Vector3 point, Vector3 axis, float delay)
		{
			float ttl = 0;
			do
			{
				transform.RotateAround(point, axis, Time.fixedDeltaTime * EVADING_SPEED * EVADING_ANGLE);
				ttl += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			while(ttl < delay);
		}

		public Character FindNearestTarget(float radius = Mathf.Infinity)
		{
			var ray = new Ray(transform.position, transform.forward);
			var hits = Physics.SphereCastAll(ray, 0.5f, radius);
			Character nearest_target = null;

			for(int i = 0; i < hits.Length; ++i)
			{
				var hit = hits[i];
				var target = hit.collider.gameObject.GetComponent<Character>();
				if(target != null && target != this)
				{
					if(nearest_target != null)
					{
						var current_dist = Vector3.Distance(transform.position, nearest_target.transform.position);
						var new_dist = Vector3.Distance(transform.position, target.transform.position);
						if(new_dist < current_dist)
						{
							nearest_target = target;
							continue;
						}
					}
					nearest_target = target;
				}
			}

			return nearest_target;
		}

		public Character TryDamage(float radius, float min, float max, bool wait_for_distance = false)
		{
			var nearest_target = FindNearestTarget(radius);

			if(nearest_target != null && !nearest_target.is_invulnerable)
			{
				if(wait_for_distance)
				{
					var dist = Vector3.Distance(transform.position, nearest_target.transform.position);
					StartCoroutine(Main.WaitAndDo(
						() => {
							TryDamage(radius, min, max);
						}, 
						dist / (mctl.speed * 1.5f)
					));
				}
				else
					Damage(nearest_target, min, max);
			}

			return nearest_target;
		}

		void Damage(Character damaged, float min, float max)
		{
			var damage = Random.Range(min, max);
			damaged.HP -= damage;
			if(hud != null)
				hud.ShowDamageInfo(damage, damaged.transform);
		}

		public void Spawn()
		{
			transform.position = GameObject.Find("startpoint_1").transform.position;
			gameObject.SetActive(true);
			OnSpawned();
		}

		public void Release()
		{
			gameObject.SetActive(false);
			OnReleased();
		}

		public void ShowVFX(string name)
		{
			var vfx = gameObject.GetChild(name).GetComponent<ParticleSystem>();
			if(vfx != null)
				vfx.Play();
		}

		public void HideVFX(string name)
		{
			var vfx = gameObject.GetChild(name).GetComponent<ParticleSystem>();
			if(vfx != null)
				vfx.Stop();
		}

		public void PlayAnim(string statename, float delay)
		{
			if(aab_defer_routine != null)
			{
				StopCoroutine(aab_defer_routine);
				aab_defer_routine = null;
			}
			animator.Play(statename, 0);
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

		void OnDie()
		{
			hp = is_dummy ? MAX_HP : 0;
		}

		void OnSpawned()
		{
			if(cam == null)
				InitCamera();

			is_spawned = true;

			cam.SetMainTarget(transform);
			cam.lockCamera = false;
			cam.cutsceneMode = false;

			var bird = Main.self.bird;
			if(bird != null)
				bird.SetActive(false);
		}

		void OnReleased()
		{
			if(cam == null)
				InitCamera();

			is_spawned = false;

			var bird = Main.self.bird;
			if(bird != null)
			{
				bird.SetActive(true);
				cam.SetMainTarget(bird.transform);
				cam.lockCamera = true;
				cam.cutsceneMode = true;
			}
		}
	}
}
