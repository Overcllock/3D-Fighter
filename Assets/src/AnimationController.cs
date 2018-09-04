using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{ 
    public class AnimationController : MonoBehaviour
	{
		Coroutine aab_defer_routine = null;

		Animator animator = null;
		Character character = null;
		float anim_speed;

		public void Init(Animator animator, Character character)
		{
			this.animator = animator;
			this.character = character;
			anim_speed = animator.speed;
		}

		public void FreezeAnim()
		{
			anim_speed = animator.speed;
			animator.speed = 0f;
		}

		public void UnfreezeAnim()
		{
			animator.speed = anim_speed;
		}

		public void PlayAnim(string statename, float delay)
		{
			if(aab_defer_routine != null)
			{
				StopCoroutine(aab_defer_routine);
				aab_defer_routine = null;
			}
			animator.Play(statename, 0);
			aab_defer_routine = StartCoroutine(AnimDefer(delay));
		}

		IEnumerator AnimDefer(float delay = 0.0f)
		{
			yield return new WaitForSecondsRealtime(delay);
			if(character.active_ability != null)
			{
				character.active_ability.Defer();
				character.active_ability = null;
			}
			aab_defer_routine = null;
		}
	}
}
