using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
	public static class Extensions
	{
		public static void CrossFadeAlpha<T>(this GameObject o, float alpha, float duration, bool ignore_time_scale = false) where T : Graphic
		{
			if(o != null)
				o.GetComponent<T>().CrossFadeAlpha(alpha, duration, ignore_time_scale);
		}

		public static GameObject GetChild(this GameObject o, string name)
		{
			Transform t = o.transform.Find(name);
			if(t == null)
				Debug.LogError("Child not found " + name);
			return t.gameObject;
		}

		public static GameObject CreateChild(this GameObject o, UnityEngine.Object prefab)
		{
			return GameObject.Instantiate(prefab, o.transform) as GameObject;
		}

		public static T AddComponentOnce<T>(this GameObject self) where T : Component
		{
			T c = self.GetComponent<T>();
			if(c == null)
				c = self.AddComponent<T>();
			return c;
		}

		public static Transform FindRecursive(this Transform current, string name)   
		{
			for(int i = 0; i < current.childCount; ++i)
			{
				var chld = current.GetChild(i); 
				var tmp = chld.FindRecursive(name);
				if(tmp != null)
					return tmp;
			}
			
			if(current.parent)
			{
				if(current.parent.Find(name) == current)
					return current;
			}
			else if(current.name == name)
				return current;

			return null;
		}
	}

	public class Error
	{
		public static void Assert(bool condition)
		{
			if(!condition) throw new Exception();
		}

		public static void Assert(bool condition, string message)
		{
			if(!condition) throw new Exception(message);
		}
	}

	public static class JSON
	{
		public static List<T> ReadDirectory<T>(string dir_path)
		{
			var files = Directory.GetFiles(dir_path, "*.json", SearchOption.AllDirectories);
			var objects = new List<T>();
			for(int i = 0; i < files.Length; ++i)
			{
				var filepath = files[i];
				T obj = default(T);

				try 
				{
					obj = JSON.ReadConfig<T>(filepath);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.Message);
				}
				
				if(obj != null)
					objects.Add(obj);
			}
			return objects;
		}

		public static T ReadConfig<T>(string path)
		{
			T data = default(T);
			using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
			{
				data = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
			}
			return data;
		}

		public static void WriteConfig<T>(string path, T obj)
		{
			using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write)))
			{
				string data = JsonConvert.SerializeObject(obj, Formatting.Indented);
				writer.Write(data);
			}
		}
	}
}
