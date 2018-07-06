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

		public uint rate;
		public EnumLeague league;
		public uint wins;
		public uint loses;
		public float winrate;

		public Account()
		{
			rate = 1000;
			league = EnumLeague.BRONZE;
		}

		public static Account Load()
		{
			//TODO: load account
			return new Account();
		}

		public static void Save(Account account)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(new FileStream(SAVEFILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
				{
					var json_str = JsonConvert.SerializeObject(account, Formatting.Indented);
					writer.Write(json_str);
				}
				Debug.Log("Account saved successfully.");
			}
			catch (Exception ex)
			{
				Debug.LogError("Account not saved: " + ex.Message);
			}
		}
	}
}
