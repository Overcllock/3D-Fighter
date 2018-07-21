namespace game
{
    public class RightEvade : Ability
	{
		public RightEvade()
		{
			cooldown_ttl = 6;
			key = EnumAbilitesKeys.KEY_E;
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