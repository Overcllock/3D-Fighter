using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public static class Extensions
	{
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

		public static Transform FindRecursive(this Transform current, string name, bool depth_first = true)   
		{
			if(!depth_first)
			{
				if(current.parent)
				{
					if(current.parent.Find(name) == current)
						return current;
				}
				//NOTE: switching to mem-allocating version only if there's no parent
				else if(current.name == name)
					return current;
			}

			for(int i = 0; i < current.childCount; ++i)
			{
				var chld = current.GetChild(i); 
				var tmp = chld.FindRecursive(name);
				if(tmp != null)
					return tmp;
			}

			if(depth_first)
			{
				if(current.parent)
				{
					if(current.parent.Find(name) == current)
						return current;
				}
				//NOTE: switching to mem-allocating version only if there's no parent
				else if(current.name == name)
					return current;
			}

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
}
