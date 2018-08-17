namespace game
{
    public class Screwkick : Ability
	{
		const string CONF_PATH = "Assets/src/abilites/screwkick/conf.json";

		public Screwkick(Character inflictor) : base()
		{
			this.inflictor = inflictor;
			ReadConf(CONF_PATH);
		}

		protected override void Use()
		{
			base.Use();
			inflictor.mctl.moving_allowed = false;
			inflictor.TryDamage(conf.radius, conf.damage_min, conf.damage_max, wait_for_distance: true);
		}

		public override bool CheckConditions()
		{
			return true;
		}

		public override void Defer()
		{
			inflictor.mctl.moving_allowed = true;
		}
	}
}