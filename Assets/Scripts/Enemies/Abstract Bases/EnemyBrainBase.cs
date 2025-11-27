using UnityEngine;

public abstract class EnemyBrainBase : MonoBehaviour, IUpdatable
{
    [Header("References")]
    [SerializeField] protected EnemyMovementBase enemyMovement;
    [SerializeField] protected EnemyAttackBase enemyAttack;

    protected Room parentRoom;

    protected UpdateManager updateManager;

    protected Transform target;

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
        enemyMovement.Init(updateManager);
    }

    public virtual void InitTarget(GameObject _target)
    {
        target = _target.transform;
        //Debug.Log(enemyAttack.name);

        if (enemyAttack != null)
            enemyAttack.InitTarget(_target);
    }

    public virtual void OnEnemyDeath()
    {
        parentRoom.OnEnemyKilled(this);
    }

    public virtual void OnUpdate()
    {
        
    }
}
