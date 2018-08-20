namespace game
{
    public class Dodge : Ability
	{
		const string CONF_PATH = "config/abilites/dodge/conf.json";

		public Dodge(Character inflictor) : base()
		{
			this.inflictor = inflictor;
			ReadConf(CONF_PATH);
		}

		protected override void Use()
		{
			//TODO:
			base.Use();
			inflictor.mctl.moving_allowed = false;
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}

		public override void Defer()
		{
			inflictor.mctl.moving_allowed = true;
		}
	}
}