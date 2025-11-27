using UnityEngine;

public class ChasingEnemyBrain : EnemyBrainBase
{
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (target == null) return;

        enemyMovement.Move(target.position);
    }
}
