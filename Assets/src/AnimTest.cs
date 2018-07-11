using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour {
	private Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jab"))
				anim.PlayInFixedTime("Hikick", 0);
			else
				anim.Play("Jab", 0);
		}
		else if(Input.GetMouseButtonDown(1))
			anim.Play("Spinkick", 0);
	}
}
