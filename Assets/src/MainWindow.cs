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

		void OnPlay()
		{
			//TODO:
			Close();
			root.Open("prefabs/HUD");
			Main.self.player.Spawn();
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
			victory_info.text = "Побед: " + acc.wins;
			defeat_info.text = "Поражений: " + acc.loses;
			rate_info.text = "Рейтинг побед: " + acc.winrate + '%';

			switch(acc.league)
			{
				case EnumLeague.SILVER:
					league_info.sprite = Resources.Load<Sprite>("sprites/rank_silver");
					rank_info_anim.sprite = Resources.Load<Sprite>("sprites/rank_silver_anim");
					break;
				case EnumLeague.GOLD:
					league_info.sprite = Resources.Load<Sprite>("sprites/button-rank_gold");
					rank_info_anim.sprite = Resources.Load<Sprite>("sprites/button-rank_gold-anim");
					break;
				case EnumLeague.PLATINUM:
					league_info.sprite = Resources.Load<Sprite>("sprites/rank_platinum");
					rank_info_anim.sprite = Resources.Load<Sprite>("sprites/rank-platinum_anim");
					break;
				default:
					league_info.sprite = Resources.Load<Sprite>("sprites/rank_bronze");
					rank_info_anim.sprite = Resources.Load<Sprite>("sprites/rank_bronze_anim");
					break;
			}
		}
	}
}
