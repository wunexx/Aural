using System.Collections;
using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField] protected float health;

    [Header("Effect")]
    [SerializeField] protected GameObject effectPrefab;
    [SerializeField] protected float effectDestroyTime;

    [Header("Hit Flash")]
    [SerializeField] protected float hitFlashDuration;
    [SerializeField] protected Color hitFlashColor;

    protected SpriteRenderer spriteRenderer;

    protected Color originalColor;

    protected bool isAlive = true;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public virtual void TakeDamage(float damage)
    {
        if (!isAlive) return;

        health -= damage;

        StartCoroutine(HitFlash());

        if (health <= 0)
            OnDeath();
    }

    protected virtual void OnDeath()
    {
        isAlive = false;
        if(effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, effectDestroyTime);
        }
    }

    protected virtual IEnumerator HitFlash()
    {
        spriteRenderer.color = hitFlashColor;
        yield return new WaitForSeconds(hitFlashDuration);
        spriteRenderer.color = originalColor;
    }
}
