using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float Drag;
    Vector3 Velocity;
    


    void Update()
    {
        var acceleration = new Vector3();
        acceleration.x = -(Velocity.x * Drag);
        acceleration.y = 0;

        var recentDpad = SimpleInput.GetRecentDpad();
        if (SimpleInput.GetInputActive(recentDpad))
        {
            switch (recentDpad)
            {
                case EInput.dpadLeft:
                {
                    acceleration.x -= Speed;
                    break;
                }
                case EInput.dpadRight:
                {
                    acceleration.x += Speed;
                    break;
                }
                case EInput.dpadUp:
                {
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        acceleration *= Time.deltaTime;
        Velocity += acceleration;
        transform.position += (Velocity * Time.deltaTime);
    }
}
