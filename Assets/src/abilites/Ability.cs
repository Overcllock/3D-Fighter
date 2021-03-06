﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace game
{
	public struct AbilityConf
	{
		public string name;
		public float damage_min;
		public float damage_max;
		public float delay;
		public float before_delay;
		public float radius;
		public float cooldown_ttl;
		public bool is_animlock;
		public string anim_state;
		public string axis;
	}

	public abstract class Ability 
	{
		public Character inflictor = null;
		public AbilityConf conf;
		public float cooldown;
		public float cooldown_percent
		{
			get { return conf.cooldown_ttl > 0 ? 1 - cooldown / conf.cooldown_ttl : 0; }
		}
		public bool is_available
		{
			get 
			{ 
				if(inflictor != null)
				{
					if(conf.is_animlock && inflictor.is_use_ability)
						return false;
					if(!conf.is_animlock && inflictor.active_ability == this)
						return false;
				}
				return cooldown == 0 && CheckConditions(); 
			}
		}

		public abstract bool CheckConditions();
		public virtual void Defer() { }
		public void SetCooldown() { cooldown = conf.cooldown_ttl; }

		protected virtual void Use()
		{
			inflictor.active_ability = this;
			inflictor.actl.PlayAnim(conf.anim_state, conf.delay);
			SetCooldown();
		}

		public virtual void Tick(float dt) 
		{ 
			TickCooldown();
		}

		void TickCooldown()
		{
			cooldown = Mathf.Clamp(cooldown -= Time.fixedDeltaTime, 0, Mathf.Infinity);
		}

		public bool TryUseAbility()
		{
			if(inflictor == null)
				return false;
			if(!is_available)
				return false;

			Use();
			return true;
		}

		protected void ReadConf(string conf_path)
		{
			try
			{
				conf = JSON.ReadConfig<AbilityConf>(Application.streamingAssetsPath + conf_path);
			}
			catch(Exception ex)
			{
				Debug.LogError("Can't read ability config. " + ex.Message);
			}
		}

		public static object GetFromConf(AbilityConf conf, object[] constructor_params)
		{
			Type type = Type.GetType("game." + conf.name);
			if(type != null)
			{
				var instance = Activator.CreateInstance(type, constructor_params);
				return instance;
			}
			return null;
		}
	}
}
