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
		public Character opponent;

		[HideInInspector]
		public GameObject bird;
		[HideInInspector]
		public bool is_paused = false;

		void Awake()
		{
			self = this;
		}

		void Start() 
		{
			bird = GameObject.Find("bird");
			account = Account.Load() ?? new Account();
			if(account.IsValid)
				ui_root.Open<MainWindow>();
			else
				ui_root.Open<RegWindow>();
			StartCoroutine(AutoSave());
		}

		public void SetPause(bool is_paused)
		{
			Time.timeScale = is_paused ? 0 : 1;
			this.is_paused = is_paused;

			if(is_paused)
				ui_root.Open<PauseWindow>();
			else
			{
				var pause_ui = ui_root.Find<PauseWindow>();
				if(pause_ui != null)
					pause_ui.Close();
			}
		}

		public void StartGame()
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
