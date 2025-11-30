using UnityEngine;

public class WaveProjectile : ProjectileBase
{
    [Header("Projectile")]
    [SerializeField] float frequency = 10f;
    [SerializeField] float amplitude = 0.5f;

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        Vector2 perp = new Vector2(-shootDirection.y, shootDirection.x);
        float waveOffset = Mathf.Sin(timeAlive * frequency) * amplitude;
        Vector2 position = startingPos + shootDirection * speed * timeAlive + perp * waveOffset;

        rb.MovePosition(position);
    }
}
