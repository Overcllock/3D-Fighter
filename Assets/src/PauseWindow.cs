using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class PauseWindow : UIWindow 
	{
		const float LOADING_DELAY = 0.7f;
		
		bool window_loaded = false;

		void Start()
		{
			MakeButton("but_continue", OnContinue);
			MakeButton("but_exit", OnMainMenu);
			StartCoroutine(LoadWindow());
		}

		void Update()
		{
			if(Input.GetKeyDown(KeyCode.Escape) && window_loaded)
				OnContinue();
		}

		IEnumerator LoadWindow()
		{
			yield return new WaitForSecondsRealtime(LOADING_DELAY);
			window_loaded = true;
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
			root.Open("prefabs/MainMenu");
		}
	}
}
