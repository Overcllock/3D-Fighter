namespace game
{
    public class Screwkick : Ability
	{
		const string CONF_PATH = "/config/abilites/screwkick/conf.json";

		public Screwkick(Character inflictor) : base()
		{
			this.inflictor = inflictor;
			ReadConf(CONF_PATH);
		}

		protected override void Use()
		{
			base.Use();
			inflictor.is_freeze = true;
			inflictor.TryDamage(conf.radius, conf.damage_min, conf.damage_max, wait_for_distance: true);
		}

		public override bool CheckConditions()
		{
			return !inflictor.has_control;
		}

		public override void Defer()
		{
			inflictor.is_freeze = false;
		}
	}
}