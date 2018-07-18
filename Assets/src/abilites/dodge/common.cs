namespace game
{
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
}