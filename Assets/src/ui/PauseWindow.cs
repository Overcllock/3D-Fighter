using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class PauseWindow : UIWindow 
	{
		public static readonly string PREFAB = "prefabs/Pause";

		const float LOADING_DELAY = 0.5f;
		
		bool window_loaded = false;

		void Start()
		{
			MakeButton("but_continue", OnContinue);
			MakeButton("but_exit", OnMainMenu);
			StartCoroutine(Main.WaitAndDo(() => { window_loaded = true; }, LOADING_DELAY));
		}

		void Update()
		{
			if(Input.GetKeyDown(KeyCode.Escape) && window_loaded)
				OnContinue();
		}

		void OnContinue()
		{
			Main.self.SetPause(false);
			Close();
		}

		void OnMainMenu()
		{
			Main.self.player.Release();
			Main.self.SetPause(false);
			root.CloseAll();
			root.Open<MainWindow>();
		}
	}
}
