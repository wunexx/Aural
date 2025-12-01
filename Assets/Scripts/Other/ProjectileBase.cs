using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class ProjectileBase : MonoBehaviour, IFixedUpdatable
{
    [Tooltip("Decides who is the target of projectile")]
    [SerializeField] protected bool playerProjectile = false;

    [SerializeField] protected bool destroyOnCol = true;

    protected float damage;
    protected float speed;
    protected float lifetime;

    protected Rigidbody2D rb;

    protected float timeAlive;
    protected Vector2 startingPos;
    protected Vector2 shootDirection;
    protected UpdateManager updateManager;

    [Header("Sprites")]
    [SerializeField] protected Sprite[] sprites;

    [Header("Effects")]
    [SerializeField] protected GameObject effectPrefab;
    [SerializeField] protected float effectDestroyTime = 1f;

    [Header("Sounds")]
    [SerializeField] protected AudioClip destroySound;
    [SerializeField] protected float volume = 0.15f;

    protected string targetTag;

    protected SpriteRenderer spriteRenderer;

    protected virtual void OnDisable()
    {
        if (updateManager != null)
            updateManager.RemoveFixedUpdatable(this);
    }

    public virtual void Init(Vector2 dir, UpdateManager um, float s, float l, float d)
    {
        rb = GetComponent<Rigidbody2D>();

        updateManager = um;
        um.AddFixedUpdatable(this);

        startingPos = transform.position;
        shootDirection = dir;

        damage = d;
        speed = s;
        lifetime = l;
    }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites.Length > 0)
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        targetTag = playerProjectile ? "Enemy" : "Player";

        string layer = playerProjectile ? "PlayerProjectile" : "EnemyProjectile";

        gameObject.layer = LayerMask.NameToLayer(layer);
    }

    public virtual void OnFixedUpdate()
    {
        timeAlive += Time.fixedDeltaTime;

        if (timeAlive >= lifetime)
            DestroyThis();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) || collision.CompareTag("Prop"))
        {
            HealthBase health = collision.gameObject.GetComponent<HealthBase>();

            //Debug.Log("Target: " + health.name + " Damage: " + damage);

            health.TakeDamage(damage);
        }

        if (destroyOnCol)
            DestroyThis();
    }

    void DestroyThis()
    {
        SoundManager.Instance.PlayProjectileDestroySFX(destroySound, volume);

        if(effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, effectDestroyTime);
        }
        Destroy(gameObject);
    }
}
