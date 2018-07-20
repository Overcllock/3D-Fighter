namespace game
{
    public class Escape : Ability
	{
		public Escape()
		{
			cooldown_ttl = 24;
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