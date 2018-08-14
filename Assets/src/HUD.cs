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
			gameObject.GetChild("enemy_bar").SetActive(visibility);
		}

		public void UpdateEnemyBar(Character enemy)
		{
			var hpbar = gameObject.GetChild("enemy_bar").GetChild("hpbar").GetComponent<Slider>();
			var hptxt = gameObject.GetChild("enemy_bar").GetChild("txt").GetComponent<Text>();
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

		public void ShowDamageInfo(float value, Transform t)
		{
			var damage_label_prefab = Resources.Load("prefabs/damage_value");
			var damage_label_go = gameObject.CreateChild(damage_label_prefab);

			var y_offset = Vector3.up * Random.Range(1.5f, 2.0f);
			var x_offset = Vector3.right * Random.Range(-0.5f, 0.5f);
			var w_pos = t.position + x_offset + y_offset;

			damage_label_go.GetComponent<RectTransform>().anchoredPosition = GetAnchorPosition(w_pos);
			damage_label_go.GetComponent<Text>().text = Mathf.CeilToInt(value).ToString();
			
			StartCoroutine(HideDamageInfoWithFading(
				label: damage_label_go,
				delay: 1.0f, 
				w_pos: w_pos,
				x_offset: x_offset,
				y_offset: y_offset,
				keep_pos: true, 
				target_transform: t
			));
			
		}

		IEnumerator HideDamageInfoWithFading(
			GameObject label, 
			float delay, 
			Vector3 w_pos,
			Vector3 y_offset,
			Vector3 x_offset,
			float y_speed = 0.75f, 
			float a_speed = 0.15f, 
			bool keep_pos = false, 
			Transform target_transform = null
		)
		{
			float ttl = 0;
			var rect = label.GetComponent<RectTransform>();
			var text = label.GetComponent<Text>();
			Vector2 current_pos = rect.anchoredPosition;
			float y_dist = 0;

			while(ttl < delay)
			{
				if(keep_pos && target_transform != null)
				{
					current_pos = GetAnchorPosition(target_transform.position + x_offset + y_offset);
					rect.anchoredPosition = new Vector2(current_pos.x, current_pos.y + y_dist);
				}

				y_dist += y_speed;
				rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + y_speed);
				text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - a_speed);
				ttl += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}

			Destroy(label);
		}

		Vector2 GetAnchorPosition(Vector3 world_pos)
		{
			Vector3 view_pos = Camera.main.WorldToViewportPoint(world_pos);
			var canvas_rect = root.canvas.GetComponent<RectTransform>();
			var canvas_size = canvas_rect.sizeDelta;
			Vector2 ui_pos = new Vector2(
				view_pos.x * canvas_size.x - canvas_size.x * 0.5f, 
				view_pos.y * canvas_size.y - canvas_size.y * 0.5f
			); 
			return ui_pos;
		}

		public void UpdateSkillButtonAlpha(EnumAbilitesKeys key, bool is_available)
		{
			var skill = GetSkillFromList(key);
			if(skill == null)
				return;

			var cooldown = skill.GetChild("cooldown");
			var cooldown_ttl = skill.GetChild("cooldown_ttl");

			cooldown.CrossFadeAlpha<Image>(is_available ? 1 : 0, 0.1f, false);
			cooldown_ttl.CrossFadeAlpha<Text>(is_available ? 1 : 0, 0.1f, false);
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
