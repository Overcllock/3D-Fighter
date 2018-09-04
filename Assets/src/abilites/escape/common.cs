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
			base.Use();
		}

		public override void Tick(float dt)
		{
			base.Tick(dt);
			if(inflictor.hud != null)
				inflictor.hud.UpdateSkillButtonAlpha(conf.axis, CheckConditions());
		}

		public override bool CheckConditions()
		{
			return inflictor.has_control;
		}
	}
}