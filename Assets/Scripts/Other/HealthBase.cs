using System.Collections;
using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField] protected float health = 100f;
    [SerializeField] protected int scoreCost = 0;

    [Header("Effect")]
    [SerializeField] protected GameObject effectPrefab;
    [SerializeField] protected float effectDestroyTime = 1f;

    [Header("Hit Flash")]
    [SerializeField] protected float hitFlashDuration = 0.1f;
    [SerializeField] protected Color hitFlashColor = new Color(176f / 255f, 48f / 255f, 92f / 255f, 1f);

    [Header("Heal Flash")]
    [SerializeField] protected float healFlashDuration = 0.1f;
    [SerializeField] protected Color healFlashColor = new Color(60f / 255f, 163f / 255f, 112f / 255f, 1f);

    [Header("Audio")]
    [SerializeField] AudioClip hitSound;
    [SerializeField] float hitVolume = 0.2f;
    [SerializeField] AudioClip healSound;
    [SerializeField] float healVolume = 0.2f;

    protected SpriteRenderer spriteRenderer;

    protected Color originalColor;

    protected bool isAlive = true;

    protected float maxHealth;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        maxHealth = health;
    }

    public virtual void TakeDamage(float damage)
    {
        if (!isAlive) return;

        health -= damage;

        health = Mathf.Clamp(health, 0, maxHealth);

        StartCoroutine(HitFlash());

        if(hitSound != null)
            SoundManager.Instance.PlayHitSFX(hitSound, hitVolume);

        if (health <= 0)
            OnDeath();
    }

    public virtual void Heal(float amount)
    {
        if (!isAlive) return;

        health += amount;

        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log($"Heal amount {amount} ||| Health after heal {health}");

        StartCoroutine(HealFlash());

        if (healSound != null)
            SoundManager.Instance.PlayOtherSFX(healSound, healVolume);
    }

    protected virtual void OnDeath()
    {
        isAlive = false;
        if(effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, effectDestroyTime);
        }

        PlayerScore.Instance.AddScore(scoreCost);
    }

    protected virtual IEnumerator HitFlash()
    {
        spriteRenderer.color = hitFlashColor;
        yield return new WaitForSeconds(hitFlashDuration);
        spriteRenderer.color = originalColor;
    }

    protected virtual IEnumerator HealFlash()
    {
        spriteRenderer.color = healFlashColor;
        yield return new WaitForSeconds(healFlashDuration);
        spriteRenderer.color = originalColor;
    }
}
