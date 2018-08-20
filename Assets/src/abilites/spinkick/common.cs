using UnityEngine;
using UnityEngine.Events;

namespace game
{
    public class Spinkick : Ability
	{
		const string CONF_PATH = "config/abilites/spinkick/conf.json";

		Character damaged = null;

		public Spinkick(Character inflictor) : base()
		{
			this.inflictor = inflictor;
			ReadConf(CONF_PATH);
		}

		protected override void Use()
		{
			base.Use();
			damaged = inflictor.TryDamage(conf.radius, conf.damage_min, conf.damage_max);
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