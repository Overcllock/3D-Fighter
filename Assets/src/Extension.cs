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
