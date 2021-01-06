using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Review : MonoBehaviour
{
	[SerializeField] Animator Animator;
	[SerializeField] PlayerIcon AgentIcon;
	[SerializeField] Text ReviewText;
	[SerializeField] Text RatingText;
	float IntroTimer;

	public void Setup(AudienceAgent target, bool flipped)
	{
		AgentIcon.SetIcon(target);
		Animator.SetBool("Flipped", flipped);
	}

	public void Intro(float introTime)
	{
		IntroTimer = introTime;
		Animator.SetTrigger("Intro");
	}
}