using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game 
{
	public enum EnumLeague
	{
		BRONZE,
		SILVER,
		GOLD,
		PLATINUM
	}

	public enum EnumControl
	{
		NONE,
		STUN,
		GROUND
	}

	public enum EnumAbilitesKeys
	{
		KEY_1 = KeyCode.Alpha1,
		KEY_2 = KeyCode.Alpha2,
		KEY_Q = KeyCode.Q,
		KEY_E = KeyCode.E,
		KEY_F = KeyCode.F,
		KEY_TAB = KeyCode.Tab,
		KEY_LMB_1 = 0,
		KEY_LMB_2 = 10,
		KEY_RMB = 1,
		NONE
	}
}
