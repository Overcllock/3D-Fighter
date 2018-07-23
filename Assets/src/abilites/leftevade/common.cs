using UnityEngine;

namespace game
{
    public class LeftEvade : Ability
	{
		public LeftEvade()
		{
			anim_state = "Land";
			is_animlock = true;
			cooldown_ttl = 6;
			radius = 3.0f;
			delay = 0.6f;
			key = EnumAbilitesKeys.KEY_Q;
		}

		protected override void Use()
		{
			//TODO:
			base.Use();
			inflictor.mctl.moving_allowed = false;
			inflictor.mctl.keep_camera_look_at = true;
			inflictor.StartCoroutine(inflictor.Evade(inflictor.target.transform.position, Vector3.up, delay / 2));
		}

		public override bool CheckConditions()
		{
			return inflictor.has_target && inflictor.distance_to_target <= radius;
		}

		public override void Defer()
		{
			inflictor.mctl.moving_allowed = true;
			inflictor.mctl.keep_camera_look_at = false;
		}
	}
}