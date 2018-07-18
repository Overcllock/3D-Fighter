namespace game
{
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
}