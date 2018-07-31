namespace game
{
    public class Screwkick : Ability
	{
		public Screwkick()
		{
			damage_min = 90.0f;
			damage_max = 120.0f;
			radius = 3.5f;
			cooldown_ttl = 12;
			key = EnumAbilitesKeys.KEY_2;
			anim_state = "ScrewKick";
			delay = 1.05f;
			is_animlock = true;
		}

		protected override void Use()
		{
			base.Use();
			inflictor.mctl.moving_allowed = false;
			inflictor.TryDamage(radius, damage_min, damage_max, wait_for_distance: true);
		}

		public override bool CheckConditions()
		{
			return true;
		}

		public override void Defer()
		{
			inflictor.mctl.moving_allowed = true;
		}
	}
}