using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public abstract class Ability 
	{
		public Character inflictor = null;
		public EnumAbilitesKeys key;
		public string anim_state;
		public uint cooldown = 0;
		public float delay;
		public bool is_animlock;
		public bool is_available
		{
			get
			{
				return cooldown == 0 && CheckConditions();
			}
		}

		protected virtual void Use()
		{
			cooldown = 3;
			inflictor.active_ability = this;
			inflictor.PlayAnim(anim_state, is_animlock, delay);
		}
		public abstract bool CheckConditions();
		public virtual void Defer() { }

		public void TickCooldown()
		{
			if(cooldown > 0)
				cooldown--;
		}

		public bool TryUseAbility()
		{
			if(!is_available || inflictor == null)
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
