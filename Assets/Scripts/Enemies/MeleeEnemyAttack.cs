using UnityEngine;

public class MeleeEnemyAttack : EnemyAttackBase
{
    public override void OnUpdate()
    {
        if (!HasTarget()) return;

        base.OnUpdate();

        if (CanAttack())
        {
            HealthBase health = currentTarget.GetComponent<HealthBase>();

            if (health)
                health.TakeDamage(damage);
            else
                Debug.Log("Ooops");

            OnAttack();
        }
    }
}
