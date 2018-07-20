namespace game
{
    public class Jab : Ability
	{
		public Jab()
		{
			cooldown_ttl = 1;
			key = EnumAbilitesKeys.KEY_LMB_1;
			anim_state = "Jab";
			delay = 0.3f;
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