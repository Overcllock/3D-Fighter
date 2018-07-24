﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace game
{
	public class UI : MonoBehaviour 
	{
		public List<UIWindow> windows = null;

		void Awake() 
		{
			windows = new List<UIWindow>();
		}

		public void Open(string prefab)
		{
			var ui_prefab = Resources.Load(prefab);
			var ui_window_go = gameObject.CreateChild(ui_prefab);
			var window = ui_window_go.GetComponent<UIWindow>();
			if(window != null)
				windows.Add(window);
		}

		public void CloseAll()
		{
			for(int i = 0; i < windows.Count; ++i)
			{
				var window = windows[i];
				window.Close();
			}
		}

		public T Find<T>() where T : UIWindow
		{
			for(int i = 0; i < windows.Count; ++i)
			{
				var window = windows[i];
				if(window is T)
					return window as T;
			}
			return null;
		}
	}

	public abstract class UIWindow : MonoBehaviour
	{
		protected UI root;

		protected void Awake()
		{
			root = Main.self.ui_root;
		}

		public void Close()
		{
			if(root == null) 
				return;
				
			root.windows.Remove(this);
			Destroy(gameObject);
		}

		protected void MakeButton(string path, UnityAction func)
		{
			var btn_go = transform.FindRecursive(path);
			var button = btn_go.GetComponent<Button>();
			if(button != null)
				button.onClick.AddListener(func);
		}

		protected T GetUIComponent<T>(string name) where T : Component
		{
			return transform.FindRecursive(name).GetComponent<T>();
		}
	}
}
