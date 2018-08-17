using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
	public class ErrorPopup : UIWindow 
	{
        public static readonly string PREFAB = "prefabs/ErrorPopup";

		Text error_msg;
        string message;

        new protected void Awake()
        {
            base.Awake();
            error_msg = transform.FindRecursive("error_msg").GetComponent<Text>();
        }

		void Start() 
		{
			MakeButton("but_ok", Close);
            error_msg.text = message;
		}

        public void SetErrorMessage(string message)
        {
            this.message = message;
        }
	}
}
