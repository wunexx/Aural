using UnityEngine;

[RequireComponent(typeof(EnemyBrainBase))]
public abstract class EnemyAttackBase : MonoBehaviour, IUpdatable
{
    [Header("Attack")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected float damage;
    [SerializeField] protected float cooldown;

    [Header("Other")]
    [Tooltip("Leave empty if there is no anim on weapon")]
    [SerializeField] protected Animator weaponAnimator;

    [Header("Weapon Rotation")]
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected bool rotateWeaponToTarget = true;
    [SerializeField] protected Transform weaponPivot;

    [Header("Audio")]
    [SerializeField] protected AudioClip attackSound;
    [SerializeField] protected float volume = 0.2f;

    protected float cooldownTimer;

    protected UpdateManager updateManager;

    protected GameObject currentTarget;

    protected SpriteRenderer spriteRenderer;

    protected virtual void OnDisable() { if (updateManager) updateManager.RemoveUpdatable(this); }

    public virtual void Init(UpdateManager um)
    {
        updateManager = um;
        um.AddUpdatable(this);
    }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void InitTarget(GameObject target) => currentTarget = target;

    public virtual Vector2 GetTargetPos() => currentTarget.transform.position;

    public virtual void OnUpdate()
    {
        if (cooldownTimer < cooldown)
            cooldownTimer += Time.deltaTime;

        if (rotateWeaponToTarget)
        {
            Vector2 dir = currentTarget.transform.position - transform.position;
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);

            weaponPivot.rotation = Quaternion.Lerp(
                weaponPivot.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }

        if(currentTarget.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;

            if (weaponPivot != null)
                weaponPivot.localScale = new Vector3(1, 1, 1);
        }
        else if(currentTarget.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;

            if (weaponPivot != null)
                weaponPivot.localScale = new Vector3(1, -1, 1);
        }
    }

    protected virtual bool CanAttack() => cooldownTimer >= cooldown && Vector2.Distance(transform.position, GetTargetPos()) <= attackRadius;

    protected void OnAttack()
    {
        cooldownTimer = 0;
        if(weaponAnimator)
            weaponAnimator.SetTrigger("Attack");

        if (attackSound != null)
            SoundManager.Instance.PlayEnemyAttackSFX(attackSound, volume);
    }

    public virtual bool HasTarget() => currentTarget != null;
}
