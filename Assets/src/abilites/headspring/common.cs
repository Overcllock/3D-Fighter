namespace game
{
    public class Headspring : Ability
	{
		const string CONF_PATH = "/config/abilites/headspring/conf.json";

		public Headspring(Character inflictor) : base()
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
			return !inflictor.has_control;
		}
	}
}