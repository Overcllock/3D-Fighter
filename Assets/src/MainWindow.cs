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
			MakeButton("but_exit", Main.self.ForceQuit);
		}

		public void OnPlay()
		{
			//TODO:
			Close();
			Destroy(gameObject);
			Main.self.player.Spawn();
		}
	}
}
