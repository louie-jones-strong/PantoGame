using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PhysicsRotation : MonoBehaviour
{
    public Vector3 CentreOfMassOffset;
    public float Mass = 1;
    
    Quaternion StartingRotation;

    void Awake()
    {
        StartingRotation = transform.rotation;
    }

    public void Refresh(Vector3 acceleration)
    {

        var force = Mass * acceleration;
        float rotation = (float)(2 * Math.PI * force.x * Time.deltaTime);
        rotation *= -1;
        transform.Rotate(0, 0, rotation);

    }
}
