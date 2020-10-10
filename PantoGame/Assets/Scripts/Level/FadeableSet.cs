using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FadeableSet : MonoBehaviour
{
	Renderer ObjectRender;
	float TimeSinceLastTrigger = float.MaxValue;

	void Awake()
	{
		ObjectRender = GetComponent<Renderer>();
	}

	public void TriggerFade()
	{
		Logger.Log("TriggerFade");
		TimeSinceLastTrigger = 0;
	}
	
	void Update()
	{
		TimeSinceLastTrigger += Time.deltaTime;

		if (TimeSinceLastTrigger >= 1)
		{
			SetFade(1);
		}
		else
		{
			SetFade(0.5f);
		}
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
