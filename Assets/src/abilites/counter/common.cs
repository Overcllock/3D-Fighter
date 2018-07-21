namespace game
{
    public class Counter : Ability
	{
		public Counter()
		{
			cooldown_ttl = 0;
			key = EnumAbilitesKeys.NONE;
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