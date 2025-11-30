using UnityEngine;

public class ExpandingProjectile : ProjectileBase
{
    [SerializeField] float sizeMultiplier = 1f;
    Vector2 initialSize;

    protected override void Awake()
    {
        base.Awake();
        initialSize = transform.localScale;
        transform.localScale = initialSize;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        rb.linearVelocity = shootDirection.normalized * speed;

        float scale = 1 + timeAlive * sizeMultiplier;
        transform.localScale = initialSize * scale;
    }
}
