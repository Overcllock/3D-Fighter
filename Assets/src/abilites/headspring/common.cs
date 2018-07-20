namespace game
{
    public class Headspring : Ability
	{
		public Headspring()
		{
			cooldown_ttl = 12;
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