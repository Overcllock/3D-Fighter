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

	public class Jab : Ability
	{
		public Jab()
		{
			key = EnumAbilitesKeys.KEY_LMB_1;
			anim_state = "Jab";
			delay = 0.4f;
			is_animlock = true;
		}

		protected override void Use()
		{
			//TODO:
			base.Use();
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}
	}

	public class Kick : Ability
	{
		public Kick()
		{
			key = EnumAbilitesKeys.KEY_LMB_2;
			anim_state = "Hikick";
			delay = 0.4f;
			is_animlock = false;
		}

		protected override void Use()
		{
			//TODO:
			base.Use();
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}
	}

	public class Spinkick : Ability
	{
		public Spinkick()
		{
			key = EnumAbilitesKeys.KEY_RMB;
			anim_state = "Spinkick";
			delay = 0.5f;
			is_animlock = true;
		}

		protected override void Use()
		{
			//TODO:
			base.Use();
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}
	}

	public class Dodge : Ability
	{
		public Dodge()
		{
			key = EnumAbilitesKeys.KEY_1;
			anim_state = "Land";
			delay = 0.5f;
			is_animlock = true;
		}

		protected override void Use()
		{
			//TODO:
			base.Use();
			inflictor.mctl.moving_allowed = false;
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}

		public override void Defer()
		{
			inflictor.mctl.moving_allowed = true;
		}
	}

	public class Counter : Ability
	{
		protected override void Use()
		{
			//TODO:
			base.Use();
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}
	}

	public class Screwkick : Ability
	{
		public Screwkick()
		{
			key = EnumAbilitesKeys.KEY_2;
			anim_state = "ScrewK";
			delay = 1.2f;
			is_animlock = true;
		}

		protected override void Use()
		{
			//TODO:
			base.Use();
			inflictor.mctl.moving_allowed = false;
			inflictor.StartCoroutine(inflictor.MoveForward(delay));
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}

		public override void Defer()
		{
			inflictor.mctl.moving_allowed = true;
		}
	}

	public class Headspring : Ability
	{
		protected override void Use()
		{
			//TODO:
			base.Use();
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}
	}

	public class Escape : Ability
	{
		protected override void Use()
		{
			//TODO:
			base.Use();
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}
	}

	public class LeftEvade : Ability
	{
		protected override void Use()
		{
			//TODO:
			base.Use();
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}
	}

	public class RightEvade : Ability
	{
		protected override void Use()
		{
			//TODO:
			base.Use();
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}
	}
}
