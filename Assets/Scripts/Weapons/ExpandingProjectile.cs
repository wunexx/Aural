using UnityEngine;

public class ExpandingProjectile : ProjectileBase
{
    [SerializeField] float sizeMultiplier;
    Vector2 initialSize;

    protected override void Awake()
    {
        base.Awake();

        initialSize = transform.localScale;
        transform.localScale = initialSize * timeAlive;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        Vector2 forward = shootDirection * speed * timeAlive;

        transform.localScale = initialSize * timeAlive * sizeMultiplier;

        rb.MovePosition((Vector2)transform.position + forward);
    }
}
