using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Review : MonoBehaviour
{
	[SerializeField] Animator Animator;
	[SerializeField] PlayerIcon AgentIcon;
	[SerializeField] Text ReviewText;
	[SerializeField] Text RatingValueText;
	[SerializeField] AnimationCurve DistributionValueCurve;
	[SerializeField] AnimationCurve SwapTimeValueCurve;

	public float CurrentRatingValue;
	public bool Showing = false;

	AudienceAgent Target;
	float CurrentIntroTimer = 0;
	float SwapTime;

	public void Setup(AudienceAgent target, bool flipped)
	{
		Target = target;
		AgentIcon.SetIcon(target);
		Animator.SetBool("Flipped", flipped);
	}

	public void Intro(float introTime)
	{
		Showing = true;
		CurrentIntroTimer = introTime;
		Animator.SetTrigger("Intro");
	}

	void Update()
	{
		CurrentIntroTimer -= Time.deltaTime;
		SwapTime -= Time.deltaTime;

		if (SwapTime <= 0)
		{
			SwapTime = SwapTimeValueCurve.Evaluate(CurrentIntroTimer);
			var valueDistribution = DistributionValueCurve.Evaluate(CurrentIntroTimer);

			float rating = 7.9f;

			if (valueDistribution > 0)
			{
				rating += Random.Range(-valueDistribution, valueDistribution);
			}

			rating = Mathf.Clamp(rating, 0, 10f);

			rating = Mathf.Round(rating*10)/10;
			CurrentRatingValue = rating;
			RatingValueText.text = CurrentRatingValue.ToString();
		}
	}
}