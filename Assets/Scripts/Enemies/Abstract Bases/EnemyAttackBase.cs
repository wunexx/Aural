using UnityEngine;

[RequireComponent(typeof(EnemyBrainBase))]
public abstract class EnemyAttackBase : MonoBehaviour, IUpdatable
{
    [SerializeField] protected float attackRadius;
    [SerializeField] protected float damage;
    [SerializeField] protected float cooldown;

    protected float cooldownTimer;

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
        if (cooldownTimer < cooldown)
            cooldownTimer += Time.deltaTime;
    }

    protected virtual bool CanAttack() => cooldownTimer >= cooldown;

    protected void ResetCooldown() => cooldownTimer = 0;

    public virtual bool HasTarget() => currentTarget != null;
}
