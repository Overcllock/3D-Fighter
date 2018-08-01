using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public static class Extensions
	{
		public static Ability FindByKey(this List<Ability> abs, EnumAbilitesKeys key)
		{
			for(int i = 0; i < abs.Count; ++i)
			{
				var ab = abs[i];
				if(ab.key == key)
					return ab;
			}
			return null;
		}

		public static GameObject GetChild(this GameObject o, string name)
		{
			Transform t = o.transform.Find(name);
			if(t == null)
				Debug.LogError("Child not found " + name);
			return t.gameObject;
		}

		public static GameObject CreateChild(this GameObject o, Object prefab)
		{
			return GameObject.Instantiate(prefab, o.transform) as GameObject;
		}

		public static GameObject CreateChild(this GameObject o, Object prefab, Vector3 pos, Quaternion rot)
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

	public class SkillButtonList
	{
		public delegate GameObject ByKey(KeyCode key);
		public delegate GameObject ByButton(int button);

		ByKey get_by_key;
		ByButton get_by_button;

		public SkillButtonList(ByKey get_by_key = null, ByButton get_by_button = null)
		{
			this.get_by_key = get_by_key;
			this.get_by_button = get_by_button;
		}

		public GameObject this[EnumAbilitesKeys key]
		{
			get
			{
				switch(key)
				{
					case EnumAbilitesKeys.NONE:
						return null;
					case EnumAbilitesKeys.KEY_LMB_1:
					case EnumAbilitesKeys.KEY_LMB_2:
						return get_by_button != null ? get_by_button(0) : null;
					case EnumAbilitesKeys.KEY_RMB:
						return get_by_button != null ? get_by_button(1) : null;
					default:
						return get_by_key != null ? get_by_key((KeyCode)key) : null;
				}
			}
		}
	}
}
