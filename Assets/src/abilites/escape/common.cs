namespace game
{
    public class Escape : Ability
	{
		const string CONF_PATH = "config/abilites/escape/conf.json";

		public Escape(Character inflictor) : base()
		{
			this.inflictor = inflictor;
			ReadConf(CONF_PATH);
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