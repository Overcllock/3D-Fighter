namespace game
{
    public class Kick : Ability
	{
		UnityEngine.Coroutine wait_coroutine = null;

		public Kick()
		{
			damage_min = 20.0f;
			damage_max = 30.0f;
			radius = 1.7f;
			cooldown_ttl = 0.0f;
			key = EnumAbilitesKeys.KEY_LMB_2;
			anim_state = "Hikick";
			delay = 0.45f;
			before_delay = 0.3f;
			is_animlock = false;
		}

		protected override void Use()
		{
			if(wait_coroutine == null)
				wait_coroutine = inflictor.StartCoroutine(Main.WaitAndDo(WaitAndUse, before_delay));
		}

		void WaitAndUse()
		{
			base.Use();
			inflictor.TryDamage(radius, damage_min, damage_max);
		}

		public override bool CheckConditions()
		{
			return true;
		}

		public override void Defer()
		{
			wait_coroutine = null;
		}
	}
}