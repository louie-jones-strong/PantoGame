using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FadeableSet : MonoBehaviour
{
	[SerializeField] float AttackTime = 0.25f;
	[SerializeField] float DecayTime = 0.25f;
	[SerializeField] float FadeAmount = 0.6f;

	Renderer ObjectRender;
	float TimeSinceLastTrigger = float.MaxValue;
	float TimeSinceFirstTrigger = float.MaxValue;

	void Awake()
	{
		ObjectRender = GetComponent<Renderer>();
	}

	public void TriggerFade()
	{
		if (TimeSinceLastTrigger >= DecayTime)
		{
			TimeSinceFirstTrigger = 0;
		}
		TimeSinceLastTrigger = 0;
	}
	
	void Update()
	{
		TimeSinceFirstTrigger += Time.deltaTime;
		TimeSinceLastTrigger += Time.deltaTime;

		float fadeAmount = 1f;
		if (TimeSinceFirstTrigger <= AttackTime)
		{
			fadeAmount = Mathf.Lerp(1, FadeAmount, TimeSinceFirstTrigger / AttackTime);
		}
		else
		{
			fadeAmount = Mathf.Lerp(FadeAmount, 1, TimeSinceLastTrigger / DecayTime);
		}
		SetFade(fadeAmount);

	}

	void SetFade(float value)
	{
		if (ObjectRender == null)
		{
			return;
		}

		foreach (var material in ObjectRender.materials)
		{
			material.color = new Color(material.color.r, material.color.g, material.color.b, value);
		}
	}
}
