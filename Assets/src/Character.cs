using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace game
{
	public class Character : MonoBehaviour
	{
		const float SKILLS_STORING_INTERVAL = 0.8f;
		const float EVADING_SPEED = 3.1f;

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
		Coroutine aab_defer_routine = null;

		[HideInInspector]
		public EnumControl control = EnumControl.NONE;
		[HideInInspector]
		public Character target = null;
		[HideInInspector]
		public MovementController mctl = null;
		[HideInInspector]
		public HUD hud = null;

		public List<Ability> abilites = null;
		public Ability active_ability = null;
		public Queue<Ability> skills_queue = null;

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
			skills_queue = new Queue<Ability>();
		}

		void Start()
		{
			animator = GetComponent<Animator>();
			mctl = GetComponent<MovementController>();
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
				UpdateSkillsQueue();
			}
		}

		void InitCamera()
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
				{
					ability.inflictor = this;
					abilites.Add(ability);
				}
			}
		}

		void TickAbilites()
		{
			for(int i = 0; i < abilites.Count; ++i)
			{
				var ab = abilites[i];
				ab.Tick(Time.fixedDeltaTime);
				if(hud != null)
					hud.UpdateCooldown(ab.key, ab.cooldown_percent);
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

		void UpdateSkillsQueue()
		{
			if(skills_queue.Count == 0)
			{
				queue_duration = 0;
				return;
			}

			queue_duration += Time.fixedDeltaTime;
			if(queue_duration >= SKILLS_STORING_INTERVAL)
			{
				skills_queue.Dequeue();
				queue_duration = 0;
				if(skills_queue.Count == 0)
					return;
			}

			var skill = skills_queue.Dequeue();
			if(!skill.TryUseAbility())
				if(!skills_queue.Contains(skill))
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
					var ab = abilites.FindByKey((EnumAbilitesKeys)key);
					if(ab != null && !ab.TryUseAbility())
					{
						if(!skills_queue.Contains(ab))
							skills_queue.Enqueue(ab);
					}
				}
				if(hud != null)
					hud.PushSkill((EnumAbilitesKeys)key, is_held);
			}

			if(Input.GetKeyDown(KeyCode.Escape))
			{
				Main.self.SetPause(!Main.self.is_paused);
				cam.SetCursorVisibility(Main.self.is_paused);
			}

			//Mouse
			for(int i = (int)EnumAbilitesKeys.KEY_LMB_1; i <= (int)EnumAbilitesKeys.KEY_RMB; ++i)
			{
				bool is_held = Input.GetMouseButton(i);
				Ability ab = null;
				if(is_held)
				{
					ab = abilites.FindByKey((EnumAbilitesKeys)i);
					if(i == (int)EnumAbilitesKeys.KEY_LMB_1 && active_ability == ab)
						ab = abilites.FindByKey(EnumAbilitesKeys.KEY_LMB_2);
					if(ab != null && !ab.TryUseAbility())
					{
						if(!skills_queue.Contains(ab))
							skills_queue.Enqueue(ab);
					}
				}
				if(hud != null)
					hud.PushSkill((EnumAbilitesKeys)i, is_held);
			}
		}

		public IEnumerator Evade(Vector3 point, Vector3 axis, float delay)
		{
			float ttl = 0;
			do
			{
				transform.RotateAround(point, axis, Time.fixedDeltaTime * EVADING_SPEED * 180.0f);
				ttl += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			while(ttl <= delay);
		}

		public Character TryDamage(float radius, float min, float max, bool wait_for_distance = false)
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
			OnRelease();
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
			HP = is_dummy ? MAX_HP : 0;
		}

		void OnSpawned()
		{
			if(cam == null)
				InitCamera();

			is_spawned = true;

			cam.SetMainTarget(transform);
			cam.lockCamera = false;
			cam.cutsceneMode = false;

			if(Main.self.bird != null)
				Main.self.bird.SetActive(false);
		}

		void OnRelease()
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
