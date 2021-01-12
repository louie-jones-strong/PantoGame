using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAgent : Agent
{
	public int ControlType {set; private get;} = -1;//todo this should be enum
	public float Speed;
	public float DashSpeedMultiplier;
	public float DashCoolDown = 1.5f;
	public float DashAffectTime = 0.5f;
	public float Drag;
	Vector3 Velocity;

	float TimeLeftOfDash;
	float DashCoolDownLeft;

	Interactable CurrentTask;

	protected override void Start()
	{
		CameraController.AddTarget(transform, weighting:Settings.PlayerCamWeighting);
		SetColour(Color.yellow);
		base.Start();
	}
	
	protected override void Update()
	{
		var acceleration = Vector3.zero;

		if (CurrentTask == null && PropSlot == null)
		{
			acceleration = UpdateMovement();
			if (IsInputInState(eInput.Interact, eButtonState.Pressed))
			{
				float minDistance = float.MaxValue;
				Prop closestProp = null;
				foreach (var prop in Prop.PropsList)
				{
					var distance = DistanceUtility.Get2d(prop.transform, transform);
					if (CanHoldProp(prop, distance:distance) && distance < minDistance)
					{
						closestProp = prop;
						minDistance = distance;
					}
				}

				if (closestProp != null)
				{
					closestProp.PickUpProp(this);
				}


				if (PropSlot == null)
				{
					foreach (var task in Interactable.Interactables)
					{
						var distance = (transform.position-task.transform.position).magnitude;
						if (distance <= minDistance && task.CanInteract(transform.position))
						{
							CurrentTask = task;
							minDistance = distance;
							CurrentTask.StartInteraction(this);
						}
					}
				}
			}
		}
		else if (PropSlot != null)
		{
			acceleration = UpdateMovement();
			if (IsInputInState(eInput.Interact, eButtonState.Pressed))
			{
				PropSlot.DropProp();
			}
		}
		else if (CurrentTask != null)
		{			
			Debug.DrawLine(transform.position, CurrentTask.transform.position, Color.green);

			var distance = (transform.position-CurrentTask.transform.position).magnitude;
			if (!CurrentTask.CanInteract(transform.position) ||
				IsInputInState(eInput.Interact, eButtonState.Pressed))
			{
				CurrentTask.EndInteraction();
				CurrentTask = null;
			}

			if (CurrentTask != null)
			{
				acceleration -= Velocity;
				Velocity = Vector3.zero;
			}
		}
		UpdateVisuals(acceleration, Velocity);
		TryHideObjectsHiddingPlayer();
		base.Update();
	}

	Vector3 UpdateMovement()
	{
		var acceleration = Vector3.zero;
		acceleration.x = GetInputValue(eInput.XAxis) * Speed;
		acceleration.z = GetInputValue(eInput.YAxis) * Speed;

		if (IsInputInState(eInput.Dash, eButtonState.Pressed) &&
			DashCoolDownLeft <= 0)
		{
			TimeLeftOfDash = DashAffectTime;
			DashCoolDownLeft = DashCoolDown;
		}

		if (TimeLeftOfDash > 0)
		{
			acceleration *= DashSpeedMultiplier;
			TimeLeftOfDash -= Time.deltaTime;
		}
		if (DashCoolDownLeft > 0)
		{
			DashCoolDownLeft -= Time.deltaTime;
		}

		acceleration += -(Velocity * Drag);

		acceleration *= Time.deltaTime;
		Velocity += acceleration;

		var moveAmount = Velocity * Time.deltaTime;
		NavMeshAgent.Move(moveAmount);
		return acceleration;
	}

	void TryHideObjectsHiddingPlayer()
	{
		var startPos = CameraController.Instance.Camera.transform.position;
		var endPos = BodyCenter.position;

		var delta = endPos-startPos;

		var hits = Physics.RaycastAll(startPos, delta, delta.magnitude);

		int found = 0;
		if (hits != null)
		{
			foreach (var hit in hits)
			{
				if (hit.transform != transform)
				{
					found += 1;
					var fadeableSet = hit.transform.GetComponent<FadeableSet>();
					if (fadeableSet != null)
					{
						fadeableSet.TriggerFade();
					}
				}
			}
		}

		Debug.DrawRay(startPos, delta, found > 0 ? Color.red : Color.green);
	}

#region Get Control input values
	public bool IsInputInState(eInput input, eButtonState state)
	{
		if (!Theatre.CanPlayersMove())
		{
			return false;
		}
		return SimpleInput.IsInputInState(input, state, index:ControlType);
	}

	public float GetInputValue(eInput input)
	{
		if (!Theatre.CanPlayersMove())
		{
			return 0f;
		}
		return SimpleInput.GetInputValue(input, index:ControlType);
	}

	public bool GetInputActive(eInput input)
	{
		if (!Theatre.CanPlayersMove())
		{
			return false;
		}
		return SimpleInput.GetInputActive(input, index:ControlType);
	}
#endregion
}
