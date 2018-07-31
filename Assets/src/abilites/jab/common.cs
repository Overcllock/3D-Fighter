using UnityEngine;

namespace game
{
    public class Jab : Ability
	{
		public Jab()
		{
			damage_min = 10.0f;
			damage_max = 15.0f;
			radius = 1.7f;
			cooldown_ttl = 1.0f;
			key = EnumAbilitesKeys.KEY_LMB_1;
			anim_state = "Jab";
			delay = 0.5f;
			is_animlock = true;
		}

		protected override void Use()
		{
			base.Use();
			inflictor.TryDamage(radius, damage_min, damage_max);
		}

		public override bool CheckConditions()
		{
			return true;
		}
	}
}