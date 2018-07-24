using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public abstract class Ability 
	{
		public Character inflictor = null;
		public EnumAbilitesKeys key = EnumAbilitesKeys.NONE;
		public string anim_state = "Idle";
		public float radius = 0;
		public float cooldown = 0;
		protected float cooldown_ttl = 0;
		public float delay;
		public float before_delay;
		public bool is_animlock = true;

		public float cooldown_percent
		{
			get { return cooldown_ttl > 0 ? 1 - cooldown / cooldown_ttl : 0; }
		}
		public bool is_available
		{
			get 
			{ 
				if(inflictor != null && is_animlock && inflictor.is_use_ability)
					return false;
				return cooldown == 0 && CheckConditions(); 
			}
		}

		public abstract bool CheckConditions();
		public virtual void Defer() { }
		public void SetCooldown() { cooldown = cooldown_ttl; }

		protected virtual void Use()
		{
			inflictor.active_ability = this;
			inflictor.PlayAnim(anim_state, is_animlock, delay);
			SetCooldown();
		}

		public virtual void Tick(float dt) 
		{ 
			TickCooldown();
		}

		void TickCooldown()
		{
			cooldown = Mathf.Clamp(cooldown -= Time.fixedDeltaTime, 0, Mathf.Infinity);
		}

		public bool TryUseAbility()
		{
			if(!is_available)
				return false;
			if(inflictor == null)
				return false;
			if(!is_animlock && inflictor.active_ability == this)
				return false;

			Use();
			return true;
		}

		public static Ability GetByKey(EnumAbilitesKeys key)
		{
			switch(key)
			{
				case EnumAbilitesKeys.KEY_1:
					return new Dodge();
				case EnumAbilitesKeys.KEY_2:
					return new Screwkick();
				case EnumAbilitesKeys.KEY_LMB_1:
					return new Jab();
				case EnumAbilitesKeys.KEY_LMB_2:
					return new Kick();
				case EnumAbilitesKeys.KEY_RMB:
					return new Spinkick();
				case EnumAbilitesKeys.KEY_F:
					return new Headspring();
				case EnumAbilitesKeys.KEY_TAB:
					return new Escape();
				case EnumAbilitesKeys.KEY_E:
					return new RightEvade();
				case EnumAbilitesKeys.KEY_Q:
					return new LeftEvade();
				default:
					return null;
			}
		}
	}
}
