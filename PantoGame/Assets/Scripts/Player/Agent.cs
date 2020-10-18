using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Agent : MonoBehaviour
{
	[SerializeField] protected NavMeshAgent NavMeshAgent;
	[SerializeField] protected Transform Root;
	[SerializeField] protected Transform BodyCenter;
	[SerializeField] protected Animator PlayerAnimator;
	[SerializeField] protected float MaxMoveSpeed = 7.5f;
    protected PhysicsRotation[] PhysicsParts;

	Vector3 LastVelocity;
	Vector3 LastPos;
    
    protected virtual void Start()
    {
        PhysicsParts = GetComponentsInChildren<PhysicsRotation>();
    }

	void Update()
	{
		UpdateVisuals();
	}
	void LateUpdate()
	{
		Root.localEulerAngles = -transform.localEulerAngles + (CameraController.Instance.Camera.transform.eulerAngles * Settings.RotateToCamMultiplier);
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
		PlayerAnimator.speed = Mathf.Clamp01(Mathf.Abs(velocity.x) / MaxMoveSpeed);
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
