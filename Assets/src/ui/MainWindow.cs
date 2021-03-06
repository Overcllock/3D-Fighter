﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
	public class MainWindow : UIWindow
	{
		public static readonly string PREFAB = "prefabs/MainMenu";

		void Start() 
		{
			Fill();
			MakeButton("but_play", OnPlay);
			MakeButton("but_exit", Main.self.ForceQuit);
		}

		void OnPlay()
		{
			Close();
			StartCoroutine(StartGame());
		}

		IEnumerator StartGame()
		{
			var wfeof = new WaitForEndOfFrame();
			Main.self.StartGame();
			while(Main.self.player == null || Main.self.opponent == null)
				yield return wfeof;
			root.Open<HUD>();
			Main.self.opponent.Spawn();
			Main.self.player.Spawn();
			Main.self.SyncCharactersRotation();
		}

		void Fill()
		{
			var acc = Main.self.account;

			var rank_info = GetUIComponent<Text>("rank_txt");
			var victory_info = GetUIComponent<Text>("victory_txt");
			var defeat_info = GetUIComponent<Text>("defeat_txt");
			var rate_info = GetUIComponent<Text>("rate_txt");
			var league_info = GetUIComponent<Image>("rank");
			var rank_info_anim = GetUIComponent<Image>("rank_anim");

			rank_info.text = "Ранг: " + acc.rate;
			victory_info.text = acc.wins.ToString();
			defeat_info.text = acc.loses.ToString();
			rate_info.text = acc.winrate.ToString() + '%';

			var conf_data = JSON.ReadConfig<Dictionary<EnumLeague, string[]>>(Application.streamingAssetsPath + "/config/league_sprites.json");
			league_info.sprite = Resources.Load<Sprite>(conf_data[acc.league][0]);
			rank_info_anim.sprite = Resources.Load<Sprite>(conf_data[acc.league][1]);
		}
	}
}
