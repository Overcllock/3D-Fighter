using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace game 
{
	public class Main : MonoBehaviour 
	{
		const float AUTOSAVE_INTERVAL = 60.0f;

		public static Main self = null;

		public Account account = null;
		public UI ui_root;
		public Character player;

		[HideInInspector]
		public GameObject bird;
		[HideInInspector]
		public bool is_paused = false;

		[HideInInspector]
		public KeyCode[] all_keys;
		[HideInInspector]
		public EnumAbilitesKeys[] all_abilites_keys;

		void Awake()
		{
			self = this;

			all_keys = new KeyCode[] 
			{
				(KeyCode)EnumAbilitesKeys.KEY_1,
				(KeyCode)EnumAbilitesKeys.KEY_2,
				(KeyCode)EnumAbilitesKeys.KEY_E,
				(KeyCode)EnumAbilitesKeys.KEY_F,
				(KeyCode)EnumAbilitesKeys.KEY_Q,
				(KeyCode)EnumAbilitesKeys.KEY_TAB
			};

			all_abilites_keys = new EnumAbilitesKeys[] 
			{
				EnumAbilitesKeys.KEY_LMB_1,
				EnumAbilitesKeys.KEY_LMB_2,
				EnumAbilitesKeys.KEY_RMB,
				EnumAbilitesKeys.KEY_1,
				EnumAbilitesKeys.KEY_2,
				EnumAbilitesKeys.KEY_E,
				EnumAbilitesKeys.KEY_F,
				EnumAbilitesKeys.KEY_Q,
				EnumAbilitesKeys.KEY_TAB
			};
		}

		void Start() 
		{
			bird = GameObject.Find("bird");

			account = Account.Load() ?? new Account();
			if(account.IsValid)
				ui_root.Open("prefabs/MainMenu");
			else
				ui_root.Open("prefabs/SignUp");

			StartCoroutine(AutoSave());
		}

		public void SetPause(bool is_paused)
		{
			Time.timeScale = is_paused ? 0 : 1;
			this.is_paused = is_paused;

			if(is_paused)
				ui_root.Open("prefabs/Pause");
			else
			{
				var pause_ui = ui_root.Find<PauseWindow>();
				if(pause_ui != null)
					pause_ui.Close();
			}
		}

		public void ForceQuit()
		{
			Application.Quit();
		}

		public void OnApplicationQuit()
		{
			if(account != null)
				Account.Save(account);
		}

		public static IEnumerator WaitAndDo(UnityAction func, float delay)
		{
			yield return new WaitForSecondsRealtime(delay);
			func();
		}

		IEnumerator AutoSave(bool repeat = true)
		{
			do
			{
				if(account != null && account.IsValid)
					Account.Save(account);
				yield return new WaitForSecondsRealtime(AUTOSAVE_INTERVAL);
			}
			while(repeat);
		}
	}
}
