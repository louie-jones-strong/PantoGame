using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PlayerAgent : Agent
{
	public int ControlType = -1;//todo this should be enum
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
		base.Start();
	}
    
    void Update()
    {
		var acceleration = Vector3.zero;

		if (CurrentTask == null)
		{
        	acceleration = UpdateMovement();

			if (SimpleInput.IsInputInState(eInput.Interact, eButtonState.Pressed, index: ControlType))
			{
				float minDistance = float.MaxValue;
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
		else
		{
			acceleration -= Velocity;
			Velocity = Vector3.zero;
			
			Debug.DrawLine(transform.position, CurrentTask.transform.position, Color.green);
			if (SimpleInput.IsInputInState(eInput.Interact, eButtonState.Pressed, index: ControlType))
			{
				CurrentTask.EndInteraction();
				CurrentTask = null;
			}
		}
		
		UpdateVisuals(acceleration, Velocity);
		TryHideObjectsHiddingPlayer();
		base.Update();
    }

	Vector3 UpdateMovement()
	{
		var acceleration = Vector3.zero;
		acceleration.x = SimpleInput.GetInputValue(eInput.XAxis, index: ControlType) * Speed;
		acceleration.z = SimpleInput.GetInputValue(eInput.YAxis, index: ControlType) * Speed;

		if (SimpleInput.IsInputInState(eInput.Dash, eButtonState.Pressed, index: ControlType) &&
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
}
