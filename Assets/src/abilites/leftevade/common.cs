namespace game
{
    public class LeftEvade : Ability
	{
		public LeftEvade()
		{
			cooldown_ttl = 6;
			key = EnumAbilitesKeys.KEY_Q;
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