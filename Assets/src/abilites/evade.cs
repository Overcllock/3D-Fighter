using UnityEngine;
using System.Collections;

namespace game
{
    public abstract class Evade : Ability
	{
		const float EVADING_SPEED = 3.1f;
		const float EVADING_ANGLE = 180.0f;

        protected override void Use()
		{
			base.Use();
			inflictor.is_invulnerable = true;
			inflictor.is_freeze = true;
			inflictor.mctl.keep_camera_look_at = true;
		}

		public override void Tick(float dt)
		{
			base.Tick(dt);
			if(inflictor.hud != null)
				inflictor.hud.UpdateSkillButtonAlpha(conf.axis, CheckConditions());
		}

		public override bool CheckConditions()
		{
			return !inflictor.has_control && inflictor.HasTargetInRadius(conf.radius);
		}

		public override void Defer()
		{
			inflictor.is_invulnerable = false;
			inflictor.is_freeze = false;
			inflictor.mctl.keep_camera_look_at = false;
		}

		protected IEnumerator Move(Vector3 point, Vector3 axis, float delay)
		{
			float ttl = 0;
			do
			{
				inflictor.transform.RotateAround(point, axis, Time.fixedDeltaTime * EVADING_SPEED * EVADING_ANGLE);
				ttl += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			while(ttl < delay);
		}
	}
}