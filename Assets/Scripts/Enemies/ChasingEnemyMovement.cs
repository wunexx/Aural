using UnityEngine;

public class ChasingEnemyMovement : EnemyMovementBase
{
    [SerializeField] float jitterStrength = 0.3f;
    [SerializeField] float jitterCooldown = 0.5f;

    private Vector2 currentJitter = Vector2.zero;
    private float jitterTimer = 0f;

    public override void Move(Vector2 pos)
    {
        jitterTimer += Time.deltaTime;
        if (jitterTimer >= jitterCooldown)
        {
            jitterTimer = 0f;
            currentJitter = Random.insideUnitCircle * jitterStrength;
        }

        Vector2 targetWithJitter = pos + currentJitter;

        base.Move(targetWithJitter);
    }
}
