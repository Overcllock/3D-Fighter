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

		public string name;
		public uint rate;
		public EnumLeague league;
		public uint wins;
		public uint loses;
		public float winrate;

		public bool IsValid 
		{
			get { return name != string.Empty; }
		}

		public Account()
		{
			name = string.Empty;
			rate = 1000;
			league = EnumLeague.BRONZE;
		}

		public Account(string name)
		{
			this.name = name;
			rate = 1000;
			league = EnumLeague.BRONZE;
		}

		public static Account Load()
		{
			try
			{
				Account account;
				using (StreamReader reader = new StreamReader(new FileStream(SAVEFILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
				{
					account = JsonConvert.DeserializeObject<Account>(reader.ReadToEnd());
				}

				if(account != null)
				{
					Debug.Log("Account loaded successfully.");
					return account;
				}

				throw new Exception("user data is null or empty.");
			}
			catch (Exception ex)
			{
				Debug.LogError("Account not loaded: " + ex.Message);
				return new Account();
			}
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
