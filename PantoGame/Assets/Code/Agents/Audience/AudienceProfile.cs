using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AudienceProfileData
{
	public string Name;
	public int Age;

	public Dictionary<eAudienceIntent, IntentTransition> IntentTransitions = new Dictionary<eAudienceIntent, IntentTransition>();

	[Serializable]
	public class IntentTransition
	{
		public float TransitionTime;
		public float RatingTimeMultiplier;
	}

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
}
