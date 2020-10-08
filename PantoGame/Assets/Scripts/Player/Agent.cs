using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Agent : MonoBehaviour
{
	[SerializeField] protected NavMeshAgent NavMeshAgent;
	[SerializeField] protected Transform Root;
	[SerializeField] protected Animator PlayerAnimator;
    protected PhysicsRotation[] PhysicsParts;

	Vector3 LastVelocity;
	Vector3 LastPos;
    
    protected virtual void Start()
    {
        PhysicsParts = GetComponentsInChildren<PhysicsRotation>();
		CameraController.AddTarget(transform);
    }

	void Update()
	{
		UpdateVisuals();
	}
	void LateUpdate()
	{
		Root.localEulerAngles = -transform.localEulerAngles + CameraController.Instance.Camera.transform.eulerAngles;
	}

    protected void UpdateVisuals()
    {
		var pos = transform.localPosition;
		var velocity = pos - LastPos;
		var acceleration = velocity - LastVelocity;

		LastVelocity = velocity;
		LastPos = pos;

		UpdateVisuals(acceleration, velocity);
    }

	protected void UpdateVisuals(Vector3 acceleration, Vector3 velocity)
    {
        RefreshPhysicsParts(acceleration);
		PlayerAnimator.SetFloat("XV", velocity.x);
		PlayerAnimator.SetFloat("YV", velocity.z);
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
