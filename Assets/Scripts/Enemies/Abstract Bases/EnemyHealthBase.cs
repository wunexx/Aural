using UnityEngine;

[RequireComponent(typeof(EnemyBrainBase))]
public abstract class EnemyHealthBase : HealthBase
{
    protected EnemyBrainBase brain;

    protected override void Awake()
    {
        base.Awake();
        brain = GetComponent<EnemyBrainBase>();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //Debug.Log("Damage: " + damage + " Health: " + health);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        brain.OnEnemyDeath();
    }
}
