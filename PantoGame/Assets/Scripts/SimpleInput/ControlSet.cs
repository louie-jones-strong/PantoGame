using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSet
{
	Dictionary<eInput, ButtonInfo> Buttons;

	public ControlSet(string xAxis, string yAxis, string dash, string interact)
	{
		Buttons = new Dictionary<eInput, ButtonInfo>();

		Buttons[eInput.XAxis] = new ButtonInfo(xAxis, true, Settings.DeadZone);
		Buttons[eInput.YAxis] = new ButtonInfo(yAxis, true, Settings.DeadZone);
		Buttons[eInput.Dash]  = new ButtonInfo(dash, false, Settings.DeadZone);
		Buttons[eInput.Interact] = new ButtonInfo(interact, false, Settings.DeadZone);
	}

	public void Refresh(float deltaTime)
	{
		foreach (var kvp in Buttons)
		{
			kvp.Value.Refresh(deltaTime);
		}
	}

	public eButtonState GetInputState(eInput input)
	{
		return Buttons[input].State;
	}

	public float GetTimeInState(eInput input)
	{
		return Buttons[input].TimeInState;
	}

	public bool GetInputActive(eInput input)
	{
		return Buttons[input].Active;
	}

	public float GetInputValue(eInput input)
	{
		return Buttons[input].Value;
	}
}