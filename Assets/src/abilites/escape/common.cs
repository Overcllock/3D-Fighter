namespace game
{
    public class Escape : Ability
	{
		public Escape()
		{
			cooldown_ttl = 24;
			key = EnumAbilitesKeys.KEY_TAB;
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