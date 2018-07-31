namespace game
{
    public class Spinkick : Ability
	{
		public Spinkick()
		{
			damage_min = 60.0f;
			damage_max = 85.0f;
			radius = 2.0f;
			cooldown_ttl = 3;
			key = EnumAbilitesKeys.KEY_RMB;
			anim_state = "Spinkick";
			delay = 0.55f;
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