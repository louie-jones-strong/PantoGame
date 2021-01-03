using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : Interactable
{
	[SerializeField] float Acceleration = 0.05f;
	[SerializeField] float Drag = 0.4f;

	public float OpenAmount;

	float Velocity;

	protected override void Awake()
	{
		OpenAmount = 0f;
		SetOpenAmount();
		base.Awake();
	}
	
	protected override void Update()
	{
		if (CurrentUser != null)
		{
			var delta = CurrentUser.GetInputValue(eInput.YAxis) * Acceleration;

			Velocity += delta * Time.deltaTime;
		}

		Velocity -= (Velocity*Drag) * Time.deltaTime;
		OpenAmount += Velocity * Time.deltaTime;

		if (OpenAmount >= 1f)
		{
			OpenAmount = 1f;
			Velocity = 0;
		}
		else if (OpenAmount <= 0f)
		{
			OpenAmount = 0f;
			Velocity = 0;
		}
		
		SetOpenAmount();
		base.Update();
	}

	public virtual void SetOpenAmount()
	{
		
	}
}
