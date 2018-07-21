namespace game
{
    public class Headspring : Ability
	{
		public Headspring()
		{
			cooldown_ttl = 12;
			key = EnumAbilitesKeys.KEY_F;
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