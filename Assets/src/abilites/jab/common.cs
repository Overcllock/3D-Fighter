namespace game
{
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
}