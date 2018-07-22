using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
	public class RegWindow : UIWindow 
	{
		InputField username_input;

		void Start() 
		{
			MakeButton("but_save", OnSubmit);
			MakeButton("but_exit", Main.self.ForceQuit);

			username_input = transform.FindRecursive("input_name").GetComponent<InputField>();
		}

		public void OnSubmit()
		{
			string username = username_input.text;
			if(username == string.Empty)
			{
				//TODO: show popup
				Debug.LogWarning("Username field is empty.");
				return;
			}

			var account = new Account(username);
			Main.self.account = account;
			Account.Save(account);
			Close();
			root.Open("prefabs/MainMenu");
		}
	}
}
