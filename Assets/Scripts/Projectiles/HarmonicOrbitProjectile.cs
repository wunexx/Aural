using UnityEngine;
using UnityEngine.UIElements;

public class HarmonicOrbitProjectile : ProjectileBase
{
    public float orbitRadius = 0.4f;
    public float orbitSpeed = 6f;

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        Vector2 forward = shootDirection * speed * timeAlive;
        float angle = timeAlive * orbitSpeed;

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * orbitRadius;

        rb.MovePosition(startingPos + forward + offset);
    }
}
