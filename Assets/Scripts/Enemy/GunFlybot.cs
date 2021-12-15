using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFlybot : GunRobot
{
    public float flyingHeight = 2;
    public float maxFlyingHeight = 4;
    protected override void Walk(Vector3 position)
    {
        FacePosition(position);
        var targetVelocity = new Vector2(
            transform.position.x < position.x ? Speed : -Speed, 
            transform.position.y < Mathf.Min(maxFlyingHeight, position.y + flyingHeight) ? Speed : -Speed);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, MovementSmoothing);
    }

    protected override void Aim(Vector3 position)
    {

        var targetVelocity = new Vector2(
            rb2D.velocity.x,
        transform.position.y < Mathf.Min(maxFlyingHeight, position.y + flyingHeight) ? Speed : -Speed);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, MovementSmoothing);

        base.Aim(position);
    }
}
