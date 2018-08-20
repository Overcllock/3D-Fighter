using UnityEngine;

namespace game
{
    public class Jab : Ability
	{
		const string CONF_PATH = "config/abilites/jab/conf.json";

		public Jab(Character inflictor) : base()
		{
			this.inflictor = inflictor;
			ReadConf(CONF_PATH);
		}

		protected override void Use()
		{
			base.Use();
			inflictor.TryDamage(conf.radius, conf.damage_min, conf.damage_max);
		}

		public override bool CheckConditions()
		{
			return true;
		}
	}
}