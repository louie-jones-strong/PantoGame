using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public PhysicsRotation[] PhysicsParts;
    public float Speed;
    public float Drag;
    Vector3 Velocity;
    
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

        acceleration.x += -(Velocity.x * Drag);

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
