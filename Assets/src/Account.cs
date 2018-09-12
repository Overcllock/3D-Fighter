using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

namespace game 
{
	public class Account 
	{
		public static readonly string SAVEFILE_PATH = "/userdata/account.json";

		public string name;
		public uint rate;
		public uint wins;
		public uint loses;
		
		[JsonIgnore]
		public uint winrate 
		{
			get
			{
				float rate = wins > 0 ? (float)wins / (float)(wins + loses) : 0;
				return (uint)Mathf.RoundToInt(rate * 100);
			}
		}
		[JsonIgnore]
		public EnumLeague league
		{
			get
			{
				if(rate < Matching.MinLeagueRate.Silver) return EnumLeague.BRONZE;
				if(rate < Matching.MinLeagueRate.Gold) return EnumLeague.SILVER;
				if(rate < Matching.MinLeagueRate.Platinum) return EnumLeague.GOLD;
				else return EnumLeague.PLATINUM;
			}
		}
		[JsonIgnore]
		public bool IsValid 
		{
			get { return name != string.Empty; }
		}

		public Account()
		{
			name = string.Empty;
			rate = Matching.MinLeagueRate.Bronze;
		}

		public Account(string name)
		{
			this.name = name;
			rate = Matching.MinLeagueRate.Bronze;
		}

		public static Account Load()
		{
			try
			{
				if(!Directory.Exists(Application.streamingAssetsPath + "/userdata"))
					Directory.CreateDirectory(Application.streamingAssetsPath + "/userdata");

				var account = JSON.ReadConfig<Account>(Application.streamingAssetsPath + SAVEFILE_PATH);
				if(account != null)
				{
					Debug.Log("Account loaded successfully.");
					return account;
				}

				throw new Exception("User data is null or empty.");
			}
			catch (Exception ex)
			{
				Debug.LogError("Account not loaded. " + ex.Message);
				return new Account();
			}
		}

		public static void Save(Account account)
		{
			try
			{
				if(!Directory.Exists(Application.streamingAssetsPath + "/userdata"))
					Directory.CreateDirectory(Application.streamingAssetsPath + "/userdata");

				JSON.WriteConfig(Application.streamingAssetsPath + SAVEFILE_PATH, account);
				Debug.Log("Account saved successfully.");
			}
			catch (Exception ex)
			{
				Debug.LogError("Account not saved. " + ex.Message);
			}
		}
	}
}
