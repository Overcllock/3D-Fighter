namespace game
{
    public class Kick : Ability
	{
		const string CONF_PATH = "Assets/src/abilites/kick/conf.json";

		UnityEngine.Coroutine wait_coroutine = null;

		public Kick(Character inflictor) : base()
		{
			this.inflictor = inflictor;
			ReadConf(CONF_PATH);
		}

		protected override void Use()
		{
			if(wait_coroutine == null)
				wait_coroutine = inflictor.StartCoroutine(Main.WaitAndDo(WaitAndUse, conf.before_delay));
		}

		void WaitAndUse()
		{
			base.Use();
			inflictor.TryDamage(conf.radius, conf.damage_min, conf.damage_max);
		}

		public override bool CheckConditions()
		{
			return inflictor.active_ability != null && inflictor.active_ability.conf.anim_state == "Jab";
		}

		public override void Defer()
		{
			wait_coroutine = null;
		}
	}
}