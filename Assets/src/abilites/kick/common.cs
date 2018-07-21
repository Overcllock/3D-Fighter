namespace game
{
    public class Kick : Ability
	{
		public Kick()
		{
			cooldown_ttl = 0;
			key = EnumAbilitesKeys.KEY_LMB_2;
			anim_state = "Hikick";
			delay = 0.4f;
			before_delay = 0.15f;
			is_animlock = false;
		}

		protected override void Use()
		{
			//TODO:
			if(inflictor != null)
				inflictor.StartCoroutine(inflictor.WaitAndDo(base.Use, before_delay));
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}
	}
}