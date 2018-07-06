using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			//TODO: save account
		}
	}
}
