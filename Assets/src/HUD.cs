using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
	public class HUD : UIWindow 
	{
		const float HPBAR_SPEED = 3.0f;

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

		Image crosshair;

		new protected void Awake()
		{
			base.Awake();
			skill_list = new SkillButtonList(GetSkillByKey, GetSkillByMouseButton);
			skill_buttons = new List<SkillButton>();
			Main.self.player.hud = this;
		}

		void Start()
		{
			crosshair = gameObject.GetChild("crosshair").GetComponent<Image>();

			Fill();
			InitSkillButtons();
		}

		void Fill()
		{
			var pinfo = transform.FindRecursive("info_player");
			var nick_txt = pinfo.FindRecursive("nick").GetComponent<Text>();
			nick_txt.text = Main.self.account.name;
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

		public void SetCrosshairAlpha(bool has_target)
		{
			if(crosshair != null)
				crosshair.CrossFadeAlpha(has_target ? 1.0f : 0.2f, 0.5f, false);
		}

		public void SetEnemyBarVisibility(bool visibility)
		{
			gameObject.GetChild("enemy_hp").SetActive(visibility);
		}

		public void UpdateEnemyBar(Character enemy)
		{
			var hpbar = gameObject.GetChild("enemy_hp").GetChild("hpbar").GetComponent<Slider>();
			var hptxt = gameObject.GetChild("enemy_hp").GetChild("txt").GetComponent<Text>();
			if(!hpbar || !hptxt)
				return;
			
			hptxt.text = string.Format("{0} / {1}", Mathf.RoundToInt(enemy.HP), Mathf.RoundToInt(Character.MAX_HP));
			hpbar.value = Mathf.Lerp(hpbar.value, enemy.HP, Time.fixedDeltaTime * HPBAR_SPEED);
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

		public void UpdateSkillButtonAlpha(EnumAbilitesKeys key, bool is_available)
		{
			var skill = GetSkillFromList(key);
			if(skill == null)
				return;

			var cooldown = skill.GetChild("cooldown");
			if(cooldown != null)
				cooldown.GetComponent<Image>().CrossFadeAlpha(is_available ? 1 : 0, 0.1f, false);

			var cooldown_ttl = skill.GetChild("cooldown_ttl");
			if(cooldown_ttl != null)
				cooldown_ttl.GetComponent<Text>().CrossFadeAlpha(is_available ? 1 : 0, 0.1f, false);
		}

		public void UpdateCooldown(EnumAbilitesKeys key, float percent, float value)
		{
			if(key == EnumAbilitesKeys.KEY_LMB_2)
				return;

			var skill_go = GetSkillFromList(key);
			if(skill_go != null)
			{
				var cooldown = skill_go.GetChild("cooldown");
				var cooldown_ttl = skill_go.GetChild("cooldown_ttl");
				var ttl = Mathf.CeilToInt(value);

				cooldown.GetComponent<Image>().fillAmount = percent;
				cooldown_ttl.GetComponent<Text>().text = ttl > 0 ? ttl.ToString() : string.Empty;
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
