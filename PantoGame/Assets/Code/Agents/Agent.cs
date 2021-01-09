using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : PropHolder
{

#region Navigation
	[SerializeField] protected NavMeshAgent NavMeshAgent;
	[SerializeField] protected Transform Root;
	public Transform BodyCenter;
	public Transform EyesPoint;
	[SerializeField] protected float MaxMoveSpeed = 7.5f;

	Vector3 LastVelocity;
	Vector3 LastPos;
#endregion

#region looks

	[SerializeField] protected SpeechBubble SpeechBubble;
	[SerializeField] protected Animator PlayerAnimator;
	[SerializeField] protected List<SpriteToObjectLoader> PartsToColour;
	protected PhysicsRotation[] PhysicsParts;
	public Color Colour;

	float TimeUntilBlink;
	const float MinBlinkDelay = 2f;
	const float MaxBlinkDelay = 10f;


#endregion

	
	protected virtual void Start()
	{
		PhysicsParts = GetComponentsInChildren<PhysicsRotation>();
	}

	protected void SetColour(Color colour)
	{
		Colour = colour;
		foreach (var part in PartsToColour)
		{
			part.Renderer.SetColour(Colour);
		}
	}

	protected virtual void Update()
	{
	}

	protected void LateUpdate()
	{
		RotateToCam.PointAtCam(Root);
	}

	protected void UpdateVisuals()
	{
		var pos = transform.localPosition;
		var velocity = pos - LastPos;
		var acceleration = velocity - LastVelocity;
		UpdateVisuals(acceleration/Time.deltaTime, velocity/Time.deltaTime);

		LastVelocity = velocity;
		LastPos = pos;
	}

	protected void UpdateVisuals(Vector3 acceleration, Vector3 velocity)
	{
		RefreshPhysicsParts(acceleration);
		PlayerAnimator.SetFloat("XV", velocity.x);
		PlayerAnimator.SetFloat("YV", velocity.z);

		TimeUntilBlink -= Time.deltaTime;
		if (TimeUntilBlink <= 0)
		{
			PlayerAnimator.SetTrigger("Blink");
			TimeUntilBlink = Random.Range(MinBlinkDelay, MaxBlinkDelay);
		}
	}

	void RefreshPhysicsParts(Vector3 acceleration)
	{
		if (PhysicsParts == null)
		{
			Logger.LogError("found no PhysicsParts");
			return;
		}

		foreach (var part in PhysicsParts)
		{
			part.Refresh(acceleration);
		}
	}
}
