using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : Interactable
{
	[SerializeField] float Speed = 0.1f;

	public float OpenAmount;

	protected override void Awake()
	{
		OpenAmount = 0f;
		SetOpenAmount();
		base.Awake();
	}
	
	protected override void Update()
	{
		if (CurrentUser == null)
		{
			return;
		}
		var delta = SimpleInput.GetInputValue(eInput.YAxis, index: CurrentUser.ControlType) * Speed;

		OpenAmount += delta * Time.deltaTime;
		OpenAmount = Mathf.Clamp01(OpenAmount);
		
		SetOpenAmount();
		base.Update();
	}

	public virtual void SetOpenAmount()
	{
		
	}
}
