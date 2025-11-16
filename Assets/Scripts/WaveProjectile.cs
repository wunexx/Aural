using UnityEngine;

public interface IProjectile
{
    public void Init(Vector2 direction, UpdateManager _updateManager, float _speed, float _lifetime);
}
public class WaveProjectile : MonoBehaviour, IUpdatable, IProjectile
{
    [Header("Projectile")]
    [SerializeField] float frequency = 10f;
    [SerializeField] float amplitude = 0.5f;
    float speed;
    float lifetime;

    Rigidbody2D rb;

    float timeAlive;
    Vector2 startingPos;
    Vector2 shootDirection;
    UpdateManager updateManager;

    public void Init(Vector2 direction, UpdateManager _updateManager, float _speed, float _lifetime)
    {
        rb = GetComponent<Rigidbody2D>();

        updateManager = _updateManager;
        updateManager.AddUpdatable(this);

        startingPos = transform.position;
        shootDirection = direction;

        speed = _speed;
        lifetime = _lifetime;
    }

    void OnDisable() => updateManager.RemoveUpdatable(this);

    public void OnUpdate()
    {
        timeAlive += Time.deltaTime;

        Vector2 perp = new Vector2(-shootDirection.y, shootDirection.x);
        float waveOffset = Mathf.Sin(timeAlive * frequency) * amplitude;
        Vector2 position = startingPos + shootDirection * speed * timeAlive + perp * waveOffset;

        rb.MovePosition(position);

        if (timeAlive >= lifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
