using UnityEngine;
using System.Collections;

namespace game
{
    public class RightEvade : Evade
	{
		const string CONF_PATH = "config/abilites/rightevade/conf.json";

		public RightEvade(Character inflictor) : base()
		{
			this.inflictor = inflictor;
			ReadConf(CONF_PATH);
		}

		protected override void Use()
		{
			base.Use();

			var target = inflictor.FindNearestTarget(conf.radius);
			inflictor.StartCoroutine(Move(target.transform.position, -Vector3.up, conf.delay / 2));
		}
	}
}