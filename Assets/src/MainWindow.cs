using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
	public class MainWindow : UIWindow
	{
		void Start() 
		{
			Fill();
			MakeButton("but_play", OnPlay);
			MakeButton("but_exit", Main.self.ForceQuit);
		}

		public void OnPlay()
		{
			//TODO:
			Close();
			Destroy(gameObject);
			root.Open("prefabs/HUD");
			Main.self.player.Spawn();
		}

		public void Fill()
		{
			var acc = Main.self.account;

			var rank_info = GetUIComponent<Text>("rank_txt");
			var victory_info = GetUIComponent<Text>("victory_txt");
			var defeat_info = GetUIComponent<Text>("defeat_txt");
			var rate_info = GetUIComponent<Text>("rate_txt");
			var league_info = GetUIComponent<Image>("rank");

			rank_info.text = "Ранг: " + acc.rate;
			victory_info.text = "Побед: " + acc.wins;
			defeat_info.text = "Поражений: " + acc.loses;
			rate_info.text = "Рейтинг побед: " + acc.winrate + '%';

			switch(acc.league)
			{
				case EnumLeague.SILVER:
					league_info.sprite = Resources.Load<Sprite>("sprites/rank_silver");
					break;
				case EnumLeague.GOLD:
					league_info.sprite = Resources.Load<Sprite>("sprites/button-rank_gold");
					break;
				case EnumLeague.PLATINUM:
					league_info.sprite = Resources.Load<Sprite>("sprites/rank_platinum");
					break;
				default:
					league_info.sprite = Resources.Load<Sprite>("sprites/rank_bronze");
					break;
			}
		}
	}
}
