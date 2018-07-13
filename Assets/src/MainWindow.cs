using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class MainWindow : UIWindow
	{
		void Start() 
		{
			MakeButton("but_play", OnPlay);
			MakeButton("but_exit", OnExit);
		}

		public void OnPlay()
		{
			//TODO:
			Close();
			Destroy(gameObject);
			Main.self.player.Spawn();
		}

		public void OnExit()
		{
			Application.Quit();
		}
	}
}
