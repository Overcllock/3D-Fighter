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

		public static GameObject CreateChild(this GameObject o, UnityEngine.Object prefab, Vector3 pos, Quaternion rot)
		{
			return GameObject.Instantiate(prefab, pos, rot, o.transform) as GameObject;
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

		public static float ToOffset(this Vector3 vec, Transform t)
		{
			float offset = 0.0f;
			
			if(vec == -t.forward)
				offset = 180.0f;
			else if(vec == t.right)
				offset = 90.0f;
			else if(vec == -t.right)
				offset = -90.0f;

			return offset;
		}
	}

	public static class JSON
	{
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
