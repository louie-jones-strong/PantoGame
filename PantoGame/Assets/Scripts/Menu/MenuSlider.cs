using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuSlider : MenuInteractable
{
	[SerializeField] TextMesh Label;
	[SerializeField] TextMesh ValueText;

	Action<float> ChangedAction;
	Menu Menu;
	float Value;
	float Min;
	float Max;

	public void Setup(Menu menu, string label, float value, Vector2 pos, float xSize, float ySize, Action<float> changedAction=null, float min=0, float max=1)
	{
		Label.text = label;
		Value = value;
		Min = min;
		Max = max;
		ChangedAction = changedAction;

		
		base.Setup(menu, pos, xSize, ySize);
	}


	protected override void Update()
	{
		base.Update();

		float value = Value;
		if (CurrentUser != null)
		{
			if (SimpleInput.IsInputInState(eInput.YAxis, eButtonState.Pressed, index: CurrentUser.ControlType))
			{
				var yValue = SimpleInput.GetInputValue(eInput.YAxis, index: CurrentUser.ControlType);

				float delta = (Max-Min)/10;
				if (yValue < 0)
				{
					delta *= -1;
				}

				value += delta;
			}
		}

		
		value = Mathf.Clamp(value, Min, Max);

		Value = value;
		ValueText.text = $"{Value}";

		if (Value != value && ChangedAction != null)
		{
			ChangedAction.Invoke(value);
		}
	}
}