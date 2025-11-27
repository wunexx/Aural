using UnityEngine;

public class SimpleProp : HealthBase
{
    protected override void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }
}
