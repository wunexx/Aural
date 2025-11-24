using UnityEngine;

public class SimpleEnemyHealth : EnemyHealthBase
{
    //yoo finally smth not as empty as other simple enemy components

    protected override void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }
}
