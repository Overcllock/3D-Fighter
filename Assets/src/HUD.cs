using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
	public class HUD : UIWindow 
	{
		class SkillButton
		{
			public GameObject go;
			public EnumAbilitesKeys key;

			public SkillButton() { }
			public SkillButton(GameObject go, EnumAbilitesKeys key)
			{
				this.go = go;
				this.key = key;
			}
		}

		SkillButtonList skill_list;
		List<SkillButton> skill_buttons;

		new protected void Awake()
		{
			base.Awake();
			Main.self.player.hud = this;
			skill_list = new SkillButtonList(GetSkillByKey, GetSkillByMouseButton);
			skill_buttons = new List<SkillButton>();
		}

		void Start()
		{
			InitSkillButtons();
		}

		void InitSkillButtons()
		{
			for(int i = 0; i < Main.self.all_abilites_keys.Length; ++i)
			{
				var ab_key = Main.self.all_abilites_keys[i];
				var skill_go = skill_list[ab_key];
				if(skill_go != null)
					skill_buttons.Add(new SkillButton(skill_go, ab_key));
			}
		}

		public void PushSkill(EnumAbilitesKeys key, bool is_push)
		{
			var skill = GetSkillFromList(key);
			if(skill == null)
				return;

			var anim = skill.GetComponent<Animator>();
			if(anim != null && anim.gameObject.activeSelf)
			{
				if(is_push != anim.GetBool("Resize"))
					anim.Play("Resize", 0);
				else if(!is_push != anim.GetBool("ResizeToStock"))
					anim.Play("ResizeToStock", 0);

				anim.SetBool("Resize", is_push);
				anim.SetBool("ResizeToStock", !is_push);
			}
		}

		public void UpdateCooldown(EnumAbilitesKeys key, float percent)
		{
			if(key == EnumAbilitesKeys.KEY_LMB_2)
				return;

			var skill_go = GetSkillFromList(key);
			if(skill_go != null)
			{
				var cooldown = skill_go.GetChild("cooldown");
				cooldown.GetComponent<Image>().fillAmount = percent;
			}
		}

		GameObject GetSkillFromList(EnumAbilitesKeys key)
		{
			for(int i = 0; i < skill_buttons.Count; ++i)
			{
				var skill_btn = skill_buttons[i];
				if(skill_btn.key == key)
					return skill_btn.go;
			}
			return null;
		}

		GameObject GetSkillByKey(KeyCode key)
		{
			return transform.FindRecursive("but_" + key.ToString()).gameObject;
		}

		GameObject GetSkillByMouseButton(int button)
		{
			string path = button == 0 ? "but_LMB" : "but_RMB";
			return transform.FindRecursive(path).gameObject;
		}
	}
}
