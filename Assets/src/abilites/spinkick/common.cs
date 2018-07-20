namespace game
{
    public class Spinkick : Ability
	{
		public Spinkick()
		{
			cooldown_ttl = 3;
			key = EnumAbilitesKeys.KEY_RMB;
			anim_state = "Spinkick";
			delay = 0.5f;
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