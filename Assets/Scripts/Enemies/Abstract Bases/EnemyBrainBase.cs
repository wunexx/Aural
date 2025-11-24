using UnityEngine;

public abstract class EnemyBrainBase : MonoBehaviour, IUpdatable
{
    [SerializeField] protected EnemyMovementBase enemyMovement;
    [SerializeField] protected EnemyAttackBase enemyAttack;

    protected Room parentRoom;

    protected UpdateManager updateManager;

    protected virtual void OnDisable() { if (updateManager) updateManager.RemoveUpdatable(this); }

    public virtual void Init(UpdateManager um, Room room) 
    {
        parentRoom = room;

        updateManager = um;
        um.AddUpdatable(this);

        InitUpdatableComponents();
    }

    protected virtual void InitUpdatableComponents()
    {
        enemyAttack.Init(updateManager);
    }

    public virtual void InitTarget(GameObject target)
    {
        //Debug.Log(enemyAttack.name);

        if (enemyAttack != null)
            enemyAttack.InitTarget(target);
    }

    public virtual void OnEnemyDeath()
    {
        parentRoom.OnEnemyKilled(this);
    }

    public virtual void OnUpdate()
    {
        if (enemyAttack == null || enemyMovement == null) return;

        if (enemyAttack.HasTarget())
            enemyMovement.Move(enemyAttack.GetTargetPos());
    }
}
