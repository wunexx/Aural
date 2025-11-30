using UnityEngine;

public class BasicProjectile : ProjectileBase
{
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        rb.MovePosition(rb.position + shootDirection * speed * Time.fixedDeltaTime);
    }
}
