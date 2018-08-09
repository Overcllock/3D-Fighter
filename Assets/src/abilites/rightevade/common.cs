using UnityEngine;

namespace game
{
    public class RightEvade : Ability
	{
		public RightEvade()
		{
			anim_state = "Land";
			is_animlock = true;
			cooldown_ttl = 6;
			radius = 3.0f;
			delay = 0.6f;
			key = EnumAbilitesKeys.KEY_E;
		}

		protected override void Use()
		{
			//TODO:
			base.Use();
			inflictor.is_invulnerable = true;
			inflictor.mctl.moving_allowed = false;
			inflictor.mctl.keep_camera_look_at = true;

			var target = inflictor.FindNearestTarget(radius);
			inflictor.StartCoroutine(inflictor.Evade(target.transform.position, -Vector3.up, delay / 2));
		}

		public override void Tick(float dt)
		{
			base.Tick(dt);
			if(inflictor.hud != null)
				inflictor.hud.UpdateSkillButtonAlpha(key, CheckConditions());
		}

		public override bool CheckConditions()
		{
			return inflictor.HasTargetInRadius(radius);
		}

		public override void Defer()
		{
			inflictor.is_invulnerable = false;
			inflictor.mctl.moving_allowed = true;
			inflictor.mctl.keep_camera_look_at = false;
		}
	}
}