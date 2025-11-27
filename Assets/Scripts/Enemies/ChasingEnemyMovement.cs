using UnityEngine;

public class ChasingEnemyMovement : EnemyMovementBase
{
    [SerializeField] float jitterStrength = 0.3f;

    public override void Move(Vector2 pos)
    {
        if(Vector2.Distance(pos, transform.position) > enemyStopDistance * 0.9f)
            pos += Random.insideUnitCircle * jitterStrength;

        base.Move(pos);
    }
}
