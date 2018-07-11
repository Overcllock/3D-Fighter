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
	}
}
