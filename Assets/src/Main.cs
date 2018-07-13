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

		void Awake()
		{
			self = this;
		}

		void Start () 
		{
			account = Account.Load() ?? new Account();

			ui_root.Open("prefabs/MainMenu");

			StartCoroutine(AutoSave());
		}
		
		void Update () 
		{
			
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
				Account.Save(account);
				yield return new WaitForSecondsRealtime(autosave_interval);
			}
			while(repeat);
		}
	}
}
