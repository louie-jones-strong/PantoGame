using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public PhysicsRotation[] PhysicsParts;
    public float Speed;
	public float DashSpeedMultiplier;
	public float DashCoolDown = 1.5f;
	public float DashAffectTime = 0.5f;
    public float Drag;
    Vector3 Velocity;

	float TimeLeftOfDash;
	float DashCoolDownLeft;
    
    void Start()
    {
        PhysicsParts = GetComponentsInChildren<PhysicsRotation>();
    }

    void Update()
    {
        var acceleration = Vector3.zero;

        if (SimpleInput.GetInputActive(EInput.dpadLeft))
        {
            acceleration.x -= Speed;
        }
        if (SimpleInput.GetInputActive(EInput.dpadRight))
        {
            acceleration.x += Speed;
        }
		if (SimpleInput.GetInputActive(EInput.dpadDown))
        {
            acceleration.z -= Speed;
        }
        if (SimpleInput.GetInputActive(EInput.dpadUp))
        {
            acceleration.z += Speed;
        }
		if (SimpleInput.GetInputState(EInput.A) == EButtonState.Pressed &&
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

        transform.position += (Velocity * Time.deltaTime);

        RefreshPhysicsParts(acceleration);
    }

    void RefreshPhysicsParts(Vector3 acceleration)
    {
        foreach (var part in PhysicsParts)
        {
            part.Refresh(acceleration);
        }
    }
}
