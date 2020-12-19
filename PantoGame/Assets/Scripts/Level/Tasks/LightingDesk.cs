using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingDesk : Interactable
{
	public Light LightToControl;
	[SerializeField] float MinXAxis;
	[SerializeField] float MaxXAxis;
	[SerializeField] float MinYAxis;
	[SerializeField] float MaxYAxis;
	[SerializeField] float Speed = 5;
	
	public override void StartInteraction(PlayerAgent playerAgent)
	{
		CameraController.AddTarget(LightToControl.transform, 100);
		base.StartInteraction(playerAgent);
	}

	public override void EndInteraction()
	{
		CameraController.RemoveTarget(LightToControl.transform);
		base.EndInteraction();
	}

	protected void Update()
	{
		base.Update();

		if (CurrentUser == null)
		{
			return;
		}

		var velocity = Vector3.zero;

		velocity.z = -SimpleInput.GetInputValue(eInput.XAxis, index: CurrentUser.ControlType) * Speed;
		velocity.x = SimpleInput.GetInputValue(eInput.YAxis, index: CurrentUser.ControlType) * Speed;

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
