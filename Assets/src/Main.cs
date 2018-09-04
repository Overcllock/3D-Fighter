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
		[HideInInspector]
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

		public void CreateCharacter(bool is_player = false, bool is_dummy = false)
		{
			Error.Assert(is_player && player == null || !is_player && opponent == null);

			var character_prefab = Resources.Load("prefabs/character");
			var startpoint = GameObject.Find("startpoint_" + (is_player ? "1" : "2")).transform;
			var character_go = Instantiate(character_prefab) as GameObject;
			var character = character_go.AddComponentOnce<Character>();
			character.Init();
			character.is_player = is_player;
			character.is_dummy = is_dummy;
			character_go.SetActive(false);

			if(is_player)
				player = character;
			else
				opponent = character;
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

		//NOTE: temporarily, in future will be moved to Matching
		public void StartGame()
		{
			CreateCharacter(is_dummy: true);
			CreateCharacter(is_player: true);
		}

		public void SyncCharactersRotation()
		{
			Error.Assert(player != null && opponent != null);
			opponent.mctl.RotateTo(player.transform);
			player.mctl.RotateTo(opponent.transform);
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
			var vfsr = new WaitForSecondsRealtime(AUTOSAVE_INTERVAL);
			do
			{
				if(account != null && account.IsValid)
					Account.Save(account);
				yield return vfsr;
			}
			while(repeat);
		}
	}
}
