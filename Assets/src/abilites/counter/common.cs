namespace game
{
    public class Counter : Ability
	{
		public Counter()
		{
			cooldown_ttl = 0;
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