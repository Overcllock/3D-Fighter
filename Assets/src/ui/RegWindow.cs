using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
	public class RegWindow : UIWindow 
	{
		public static readonly string PREFAB = "prefabs/SignUp";
		
		const int MIN_NAME_LENGTH = 3;

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
			string errormsg = string.Empty;
			bool ok = true;

			if(username.Length == 0)
			{
				ok = false;
				errormsg = "Заполните все поля.";
			}
			else if(username.Length < MIN_NAME_LENGTH)
			{
				ok = false;
				errormsg = "Длина имени должна быть не менее " +  MIN_NAME_LENGTH + " символов.";
			}
			
			if(!ok)
			{
				root.Open<ErrorPopup>((wnd) => { 
					(wnd as ErrorPopup).SetErrorMessage(errormsg); 
				});
				Debug.LogWarning("Username field is empty or too small.");
				return;
			}

			var account = new Account(username);
			Main.self.account = account;
			Account.Save(account);
			Close();
			root.Open<MainWindow>();
		}
	}
}
