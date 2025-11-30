using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase
{
    [Header("Health UI")]
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    [Header("Other References")]
    [SerializeField] GameObject deathScreenObj;
    [SerializeField] PlayerComponentBase[] playerComponents;

    float initialHealth;

    protected override void Awake()
    {
        base.Awake();

        initialHealth = health;

        UpdateUI();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UpdateUI();
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        UpdateUI();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        foreach (var c in playerComponents)
            c.OnPlayerDeath();

        deathScreenObj.SetActive(true);
        UpdateUI();
    }

    void UpdateUI()
    {
        healthSlider.maxValue = initialHealth;
        healthSlider.value = health;

        healthText.text = $"{health}/{initialHealth}";
    }
}
