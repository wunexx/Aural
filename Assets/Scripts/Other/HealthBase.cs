using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField] protected float health;

    [SerializeField] protected GameObject effectPrefab;
    [SerializeField] protected float effectDestroyTime;

    protected virtual void Awake()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            OnDeath();
    }

    protected virtual void OnDeath()
    {
        if(effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, effectDestroyTime);
        }
    }
}
