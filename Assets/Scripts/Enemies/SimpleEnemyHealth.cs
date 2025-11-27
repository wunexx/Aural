using UnityEngine;

public class SimpleEnemyHealth : EnemyHealthBase
{
    protected override void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }
}
