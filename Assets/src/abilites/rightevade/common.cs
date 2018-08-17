using UnityEngine;

namespace game
{
    public class RightEvade : Ability
	{
		const string CONF_PATH = "Assets/src/abilites/rightevade/conf.json";

		public RightEvade(Character inflictor) : base()
		{
			this.inflictor = inflictor;
			ReadConf(CONF_PATH);
		}

		protected override void Use()
		{
			base.Use();
			inflictor.is_invulnerable = true;
			inflictor.mctl.moving_allowed = false;
			inflictor.mctl.keep_camera_look_at = true;

			var target = inflictor.FindNearestTarget(conf.radius);
			inflictor.StartCoroutine(inflictor.Evade(target.transform.position, -Vector3.up, conf.delay / 2));
		}

		public override void Tick(float dt)
		{
			base.Tick(dt);
			if(inflictor.hud != null)
				inflictor.hud.UpdateSkillButtonAlpha(conf.axis, CheckConditions());
		}

		public override bool CheckConditions()
		{
			return inflictor.HasTargetInRadius(conf.radius);
		}

		public override void Defer()
		{
			inflictor.is_invulnerable = false;
			inflictor.mctl.moving_allowed = true;
			inflictor.mctl.keep_camera_look_at = false;
		}
	}
}