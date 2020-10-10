using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eButtonState { none = -1, Pressed, Held, Released };
public enum eInput { none = -1, XAxis, YAxis, Dash};

public class SimpleInput : MonoBehaviour
{
	static SimpleInput Instance;
	static List<ControlSet> ControlSets = new List<ControlSet>();
	public static int ControlSetCount {get {return ControlSets.Count;}}

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			ControlSets.Add( new ControlSet("Horizontal_1", "Vertical_1", "Dash_1") );
			ControlSets.Add( new ControlSet("Horizontal_2", "Vertical_2", "Dash_2") );
			ControlSets.Add( new ControlSet("Horizontal_3", "Vertical_3", "Dash_3") );
			ControlSets.Add( new ControlSet("Horizontal_4", "Vertical_4", "Dash_4") );
		}
		else
		{
			enabled = false;
		}
	}

	void Update()
	{
		foreach (var controlSet in ControlSets)
		{
			controlSet.Refresh(Time.deltaTime);
		}
	}

#region  public API
	public static bool IsInputInState(eInput input, eButtonState state, int index=-1)
	{
		if (index >= ControlSetCount || index < -1)
		{
			Logger.LogError($"IsInputInState called with index({index}) out of range");
		}

		for (int loop = 0; loop < ControlSets.Count; loop++)
		{
			if (ControlSets[loop].GetInputState(input) == state && (index == -1 || index == loop))
			{
				return true;
			}
		}
		return false;
	}

	public static float GetInputValue(eInput input, int index=-1)
	{
		if (index >= ControlSetCount || index < -1)
		{
			Logger.LogError($"GetInputValue called with index({index}) out of range");
		}

		float value = 0;
		int count = 0;
		for (int loop = 0; loop < ControlSets.Count; loop++)
		{
			if (ControlSets[loop].GetInputActive(input) && (index == -1 || index == loop))
			{
				value += ControlSets[loop].GetInputValue(input);
				count += 1;
			}
		}
		
		if (count == 0)
		{
			return value;
		}
		
		return value / count;
	}

	public static bool GetInputActive(eInput input, int index=-1)
	{
		if (index >= ControlSetCount || index < -1)
		{
			Logger.LogError($"GetInputActive called with index({index}) out of range");
		}

		for (int loop = 0; loop < ControlSets.Count; loop++)
		{
			if (ControlSets[loop].GetInputActive(input) && (index == -1 || index == loop))
			{
				return true;
			}
		}
		return false;
	}
#endregion
}
