using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
	public class HUD : UIWindow 
	{
		public static readonly string PREFAB = "prefabs/HUD";

		const float HPBAR_SPEED = 3.0f;

		class SkillButton
		{
			public GameObject go;
			public string axis;

			public SkillButton(GameObject go, string axis)
			{
				this.go = go;
				this.axis = axis;
			}
		}
		List<SkillButton> skill_buttons;

		Image crosshair;

		new protected void Awake()
		{
			base.Awake();
			skill_buttons = new List<SkillButton>();
			Main.self.player.hud = this;
		}

		void Start()
		{
			crosshair = gameObject.GetChild("crosshair").GetComponent<Image>();

			Fill();
			StartCoroutine(InitSkillButtons());
		}

		void Fill()
		{
			var pinfo = transform.FindRecursive("info_player");
			var nick_txt = pinfo.FindRecursive("nick").GetComponent<Text>();
			nick_txt.text = Main.self.account.name;
		}

		IEnumerator InitSkillButtons()
		{
			yield return new WaitWhile(() => 
			{
				return Main.self.player.abilites == null || Main.self.player.abilites.Count == 0;
			});

			for(int i = 0; i < Main.self.player.abilites.Count; ++i)
			{
				var ability = Main.self.player.abilites[i];

				//temporarily
				if(ability.conf.axis == "LMB2")
					continue;

				var skill_go = GetSkillByAxis(ability.conf.axis);
				if(skill_go != null)
					skill_buttons.Add(new SkillButton(skill_go, ability.conf.axis));
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

		public void PushSkill(string axis, bool is_push)
		{
			var skill = GetSkillFromList(axis);
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

		public void UpdateSkillButtonAlpha(string axis, bool is_available)
		{
			var skill = GetSkillFromList(axis);
			if(skill == null)
				return;

			var cooldown = skill.GetChild("cooldown");
			var cooldown_ttl = skill.GetChild("cooldown_ttl");

			cooldown.CrossFadeAlpha<Image>(is_available ? 1 : 0, 0.1f, false);
			cooldown_ttl.CrossFadeAlpha<Text>(is_available ? 1 : 0, 0.1f, false);
		}

		public void UpdateCooldown(string axis, float percent, float value)
		{
			var skill_go = GetSkillFromList(axis);
			if(skill_go != null)
			{
				var cooldown = skill_go.GetChild("cooldown");
				var cooldown_ttl = skill_go.GetChild("cooldown_ttl");
				var ttl = Mathf.CeilToInt(value);

				cooldown.GetComponent<Image>().fillAmount = percent;
				cooldown_ttl.GetComponent<Text>().text = ttl > 0 ? ttl.ToString() : string.Empty;
			}
		}

		GameObject GetSkillFromList(string axis)
		{
			for(int i = 0; i < skill_buttons.Count; ++i)
			{
				var skill_btn = skill_buttons[i];
				if(skill_btn.axis == axis)
					return skill_btn.go;
			}
			return null;
		}

		GameObject GetSkillByAxis(string axis)
		{
			if(axis.Length == 0)
				return null;
			return transform.FindRecursive("but_" + axis).gameObject;
		}
	}
}
