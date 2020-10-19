using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingDesk : Interactable
{
	[SerializeField] Light LightToControl;
	[SerializeField] float MinXAxis;
	[SerializeField] float MaxXAxis;
	[SerializeField] float MinYAxis;
	[SerializeField] float MaxYAxis;
	[SerializeField] float Speed = 5;
	
	public override void Interact(int controlIndex)
	{
		var velocity = Vector3.zero;

		velocity.z = -SimpleInput.GetInputValue(eInput.XAxis, index: controlIndex) * Speed;
		velocity.x = SimpleInput.GetInputValue(eInput.YAxis, index: controlIndex) * Speed;

		MoveLight(velocity);
	}

	void MoveLight(Vector3 velocity)
	{
		var pos = LightToControl.transform.localPosition;
		
		velocity.y = 0;
		pos += velocity * Time.deltaTime;
		pos.z = Mathf.Clamp(pos.z, MinXAxis, MaxXAxis);
		pos.x = Mathf.Clamp(pos.x, MinYAxis, MaxYAxis);

		LightToControl.transform.localPosition = pos;
	}
}
