namespace game
{
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
}