using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class AudienceProfileData
{
	public string Name;
	public int Age;


	[Serializable]
	public class IntentTransition
	{
		public float TransitionTime;
		public float RatingTimeMultiplier;
	}
	public SaveableDict<eAudienceIntent, IntentTransition> IntentTransitions = new SaveableDict<eAudienceIntent, IntentTransition>();

	[Serializable]
	public class ActionResponse
	{
		public float RatingMultiplier = 1;
		public List<ReviewText> ReviewStrings;
		
	}

	public SaveableDict<eAudienceReviewableActions, ActionResponse> ActionResponses = new SaveableDict<eAudienceReviewableActions, ActionResponse>();

#region non Serializable

	public float TimeHitByPlayer {set; get;}
	public float TimeSeeingPlayerOnStage {set; get;}

	public float GetTransitionTime(eAudienceIntent intent, float rating)
	{
		if (!IntentTransitions.TryGetValue(intent, out IntentTransition intentTransition))
		{
			Logger.LogError($"IntentTransitions doesn't contain intent: \"{intent}\"");
			return UnityEngine.Random.Range(0.5f, 3f);
		}
		var time = intentTransition.TransitionTime;
		time += rating * intentTransition.RatingTimeMultiplier;
		time = Mathf.Max(time, 0f);
		return time;
	}


	//get rating value between 0 and 10
	public float GetRatingValue()
	{
		var currentScript = Theatre.Instance.CurrentScript;

		var rating = currentScript.Rating;
		rating -= TimeHitByPlayer;
		rating -= TimeSeeingPlayerOnStage;

		rating += 5;
		rating = Mathf.Clamp(rating, 0f, 10f);
		return rating;
	}

	public string GetReviewText(float rating)
	{

		// general review
		var generalReviews = DataManager.Instance.AudienceData.GeneralReviews;
		return GetReviewText(generalReviews, rating);
	}
	
	string GetReviewText(List<ReviewText> reviews, float rating)
	{
		float bestDistance = float.MaxValue;
		string bestReview =  null;
		foreach (var review in reviews)
		{
			float distance = Mathf.Abs(rating - review.ReviewRating);
			if (bestDistance > distance ||
				(bestDistance == distance && UnityEngine.Random.value >= 0.5f))
			{
				bestReview = review.ReviewString;
				bestDistance = distance;
			}
		}
		return bestReview;
	}
#endregion
}
