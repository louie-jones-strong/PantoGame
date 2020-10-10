using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eButtonState { none = -1, Pressed, Held, Released };
public enum eInput { none = -1, dpadUp, dpadRight, dpadDown, dpadLeft, A, B, Start, Select };
public class SimpleInput : MonoBehaviour
{
	static SimpleInput instance;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			for (int i = 0; i <= (int)eInput.Select; ++i)
			{
				Buttons.Add(new ButtonInfo((eInput)i));
			}
		}
		else
		{
			enabled = false;
		}
	}

	float DeadZone = 0.5f;

	public class ButtonInfo
	{
		eInput button;
		bool isDpad;

		public eButtonState State { get; private set;}
		public float TimeInState { get; private set;}
		public bool Active { get; private set; }

		public ButtonInfo(eInput b)
		{
			button = b;
			State = eButtonState.none;
			Active = false;
			isDpad = (button == eInput.dpadUp || button == eInput.dpadRight || button == eInput.dpadDown || button == eInput.dpadLeft);
		}

		public override string ToString()
		{
			return $"Active: {Active}, state: {State}";
		}

		void SetState(eButtonState state)
		{
			if (State != state)
			{
				TimeInState = 0;
			}
			State = state;
		}

		public void SetActive(bool active)
		{
			TimeInState += Time.deltaTime;

			Active = active;
			if (Active)
			{
				if (State == eButtonState.Held || State == eButtonState.Pressed)
				{
					SetState(eButtonState.Held);
				}
				else
				{
					SetState(eButtonState.Pressed);
					if(isDpad)
					{
						recentDpadInput = button;
					}
				}
			}
			else
			{
				if (State == eButtonState.Held || State == eButtonState.Pressed)
				{
					SetState(eButtonState.Released);
				}
				else
				{
					SetState(eButtonState.none);
				}
			}
		}
	}

	static List<ButtonInfo> Buttons = new List<ButtonInfo>();

	static eInput recentDpadInput;

	List<string> AxisStrings = new List<string>() {
		 "Vertical", "Horizontal" , "Vertical" ,"Horizontal" , "ButtonA", "ButtonB", "Start", "Select" };

	void Update()
	{
		Buttons[(int)eInput.dpadUp].SetActive(Input.GetAxisRaw(AxisStrings[(int)eInput.dpadUp]) > DeadZone);
		Buttons[(int)eInput.dpadRight].SetActive(Input.GetAxisRaw(AxisStrings[(int)eInput.dpadRight]) > DeadZone);
		Buttons[(int)eInput.dpadDown].SetActive(Input.GetAxisRaw(AxisStrings[(int)eInput.dpadDown]) < -DeadZone);
		Buttons[(int)eInput.dpadLeft].SetActive(Input.GetAxisRaw(AxisStrings[(int)eInput.dpadLeft]) < -DeadZone);
		for (int i = (int)eInput.A; i <= (int)eInput.Select; ++i)
		{
			Buttons[i].SetActive(Input.GetButton(AxisStrings[i]));
		}
	}

	public static eButtonState GetInputState(eInput input)
	{
		return Buttons[(int)input].State;
	}

	public static float GetTimeInState(eInput input)
	{
		return Buttons[(int)input].TimeInState;
	}

	public static bool GetInputActive(eInput input)
	{
		return Buttons[(int)input].Active;
	}

	public static eInput GetRecentDpad()
	{
		return recentDpadInput;
	}
}
