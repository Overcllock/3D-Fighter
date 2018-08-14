using UnityEngine;
using UnityEngine.Events;

namespace game
{
    public class Spinkick : Ability
	{
		Character damaged = null;

		public Spinkick()
		{
			damage_min = 60.0f;
			damage_max = 85.0f;
			radius = 2.0f;
			cooldown_ttl = 3;
			key = EnumAbilitesKeys.KEY_RMB;
			anim_state = "Spinkick";
			delay = 0.55f;
			is_animlock = true;
		}

		protected override void Use()
		{
			base.Use();
			damaged = inflictor.TryDamage(radius, damage_min, damage_max);
			if(damaged != null)
			{
				damaged.SetControl(new Character.Control(
					type: EnumControl.STUN,
					duration: 2.0f,
					vfx: "stun",
					OnStart: new UnityAction(() => {
						damaged.FreezeAnim();
					}),
					OnFinish: new UnityAction(() => {
						damaged.UnfreezeAnim();
					})
				));
			}
		}

		public override bool CheckConditions()
		{
			return true;
		}
	}
}