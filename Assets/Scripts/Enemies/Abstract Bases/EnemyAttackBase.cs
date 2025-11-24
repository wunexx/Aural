using UnityEngine;

[RequireComponent(typeof(EnemyBrainBase))]
public abstract class EnemyAttackBase : MonoBehaviour, IUpdatable
{
    [SerializeField] protected float attackRadius;
    protected UpdateManager updateManager;

    protected GameObject currentTarget;

    protected virtual void OnDisable() { if (updateManager) updateManager.RemoveUpdatable(this); }

    public virtual void Init(UpdateManager um)
    {
        updateManager = um;
        um.AddUpdatable(this);
    }

    protected virtual void Awake()
    {
        
    }

    public virtual void InitTarget(GameObject target) => currentTarget = target;

    public virtual Vector2 GetTargetPos() => currentTarget.transform.position;

    public virtual void OnUpdate()
    {
        
    }

    public virtual bool HasTarget() => currentTarget != null;
}
