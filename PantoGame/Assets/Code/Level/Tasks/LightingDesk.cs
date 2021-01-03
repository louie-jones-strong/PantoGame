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

	protected override void Update()
	{
		base.Update();

		if (CurrentUser == null)
		{
			return;
		}

		var velocity = Vector3.zero;

		velocity.z = CurrentUser.GetInputValue(eInput.YAxis) * Speed;
		velocity.x = CurrentUser.GetInputValue(eInput.XAxis) * Speed;

		MoveLight(velocity);
	}

	void MoveLight(Vector3 velocity)
	{
		var pos = LightToControl.transform.position;
		
		velocity.y = 0;
		pos += velocity * Time.deltaTime;
		pos.z = Mathf.Clamp(pos.z, MinYAxis, MaxYAxis);
		pos.x = Mathf.Clamp(pos.x, MinXAxis, MaxXAxis);

		LightToControl.transform.position = pos;
	}


#if UNITY_EDITOR

	protected override void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;

		var size = new Vector3(MaxXAxis-MinXAxis, 2, MaxYAxis-MinYAxis);

		var pos = new Vector3(
			MinXAxis+((MaxXAxis-MinXAxis)/2),
			LightToControl.transform.position.y,
			MinYAxis+((MaxYAxis-MinYAxis)/2)
			);

		Gizmos.DrawWireCube(pos, size);
		base.OnDrawGizmosSelected();
	}

#endif
}
