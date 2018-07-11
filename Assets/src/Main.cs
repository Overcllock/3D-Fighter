using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game {
	public class Main : MonoBehaviour {
		public static Main self = null;
		public Account account = null;
		public Character player = null;

		void Awake()
		{
			self = this;
		}

		void Start () 
		{
			account = Account.Load() ?? new Account();
			player = Character.Load(account);
		}
		
		void Update () 
		{
			
		}

		public void OnApplicationQuit()
		{
			if(account != null)
				Account.Save(account);
		}
	}
}
