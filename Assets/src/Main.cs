using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game {
	public class Main : MonoBehaviour {
		public static Main self = null;
		public Account account = null;
		public UI ui_root;
		public Character player;

		const float autosave_interval = 60.0f;

		public KeyCode[] all_keys;
		public EnumAbilitesKeys[] all_abilites_keys;

		void Awake()
		{
			self = this;

			all_keys = new KeyCode[] {
				(KeyCode)EnumAbilitesKeys.KEY_1,
				(KeyCode)EnumAbilitesKeys.KEY_2,
				(KeyCode)EnumAbilitesKeys.KEY_E,
				(KeyCode)EnumAbilitesKeys.KEY_F,
				(KeyCode)EnumAbilitesKeys.KEY_Q,
				(KeyCode)EnumAbilitesKeys.KEY_TAB
			};

			all_abilites_keys = new EnumAbilitesKeys[] {
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

		void Start () 
		{
			account = Account.Load() ?? new Account();
			
			if(account.IsValid)
				ui_root.Open("prefabs/MainMenu");
			else
				ui_root.Open("prefabs/SignUp");

			StartCoroutine(AutoSave());
		}
		
		void Update () 
		{
			
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

		IEnumerator AutoSave(bool repeat = true)
		{
			do
			{
				if(account != null && account.IsValid)
					Account.Save(account);
				yield return new WaitForSecondsRealtime(autosave_interval);
			}
			while(repeat);
		}
	}
}
