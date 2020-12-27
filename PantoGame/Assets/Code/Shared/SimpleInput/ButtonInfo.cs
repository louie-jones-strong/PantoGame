using UnityEngine;

public class ButtonInfo
{
	public bool Active { get {return Value != 0;}}
	public eButtonState State { get; private set;}
	public float TimeInState { get; private set;}
	public float Value { get; private set;}
	
	float DeadZone;
	bool IsAxis;
	string InputName;


	public ButtonInfo(string inputName, bool isAxis, float deadZone)
	{
		State = eButtonState.none;
		InputName = inputName;
		IsAxis = isAxis;
		DeadZone = deadZone;
	}

	public void Refresh(float deltaTime)
	{
		if (IsAxis)
		{
			SetValue(deltaTime, Input.GetAxisRaw(InputName));
		}
		else
		{
			SetValue(deltaTime, Input.GetButton(InputName)? 1: 0);
		}
	}

	void SetValue(float deltaTime, float rawValue)
	{
		TimeInState += deltaTime;

		Value = CheckDeadZone(rawValue);

		if (Active)
		{
			if (State == eButtonState.Held || State == eButtonState.Pressed)
			{
				SetState(eButtonState.Held);
			}
			else
			{
				SetState(eButtonState.Pressed);
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

	float CheckDeadZone(float rawValue)
	{
		if (rawValue >= -DeadZone && rawValue <= DeadZone)
		{
			return 0;
		}
		return rawValue;
	}	

	void SetState(eButtonState state)
	{
		if (State != state)
		{
			TimeInState = 0;
		}
		State = state;
	}

	public override string ToString()
	{
		return $"Value: {Value}, state: {State}";
	}
}