using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

namespace game {
	public class Account 
	{
		public static readonly string SAVEFILE_PATH = "userdata/account.json";
		public static readonly uint BRONZE_RATE = 1300;
		public static readonly uint SILVER_RATE = 1350;
		public static readonly uint GOLD_RATE = 1600;
		public static readonly uint PLATINUM_RATE = 1900;

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
				if(rate < SILVER_RATE) return EnumLeague.BRONZE;
				if(rate < GOLD_RATE) return EnumLeague.SILVER;
				if(rate < PLATINUM_RATE) return EnumLeague.GOLD;
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
			rate = BRONZE_RATE;
		}

		public Account(string name)
		{
			this.name = name;
			rate = BRONZE_RATE;
		}

		public static Account Load()
		{
			try
			{
				if(!Directory.Exists("userdata"))
					Directory.CreateDirectory("userdata");

				var account = JSON.ReadConfig<Account>(SAVEFILE_PATH);
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
				if(!Directory.Exists("userdata"))
					Directory.CreateDirectory("userdata");

				JSON.WriteConfig(SAVEFILE_PATH, account);
				Debug.Log("Account saved successfully.");
			}
			catch (Exception ex)
			{
				Debug.LogError("Account not saved. " + ex.Message);
			}
		}
	}
}
