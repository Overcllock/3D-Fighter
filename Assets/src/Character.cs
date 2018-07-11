using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class Character 
	{
		public bool is_moving = false;

		public Character()
		{

		}

		public static Character Load(Account data)
		{
			//TODO: load character
			return new Character();
		}
	}
}
