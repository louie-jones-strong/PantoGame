using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPiece : Interactable
{
	[SerializeField] float MinXAxis;
	[SerializeField] float MaxXAxis;
	[SerializeField] float MinYAxis;
	[SerializeField] float MaxYAxis;
	[SerializeField] float Speed = 5;
	
	protected override void Update()
	{
		base.Update();

		if (CurrentUser == null)
		{
			return;
		}

		var velocity = Vector3.zero;

		velocity.z = SimpleInput.GetInputValue(eInput.YAxis, index: CurrentUser.ControlType) * Speed;
		velocity.x = SimpleInput.GetInputValue(eInput.XAxis, index: CurrentUser.ControlType) * Speed;

		Move(velocity);
	}

	void Move(Vector3 velocity)
	{
		var pos = transform.localPosition;
		
		velocity.y = 0;
		pos += velocity * Time.deltaTime;
		
		pos.z = Mathf.Clamp(pos.z, MinYAxis, MaxYAxis);
		pos.x = Mathf.Clamp(pos.x, MinXAxis, MaxXAxis);

		CurrentUser.transform.position += pos - transform.localPosition;

		transform.localPosition = pos;
	}
	

#if UNITY_EDITOR

	protected override void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		var size = new Vector3(MaxXAxis-MinXAxis, 2, MaxYAxis-MinYAxis);

		var pos = new Vector3(
			MinXAxis+((MaxXAxis-MinXAxis)/2),
			transform.position.y,
			MinYAxis+((MaxYAxis-MinYAxis)/2)
			);

		Gizmos.DrawWireCube(pos, size);
		base.OnDrawGizmosSelected();
	}

#endif
}
