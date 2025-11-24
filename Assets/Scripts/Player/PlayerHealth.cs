using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase
{
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

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

    protected override void OnDeath()
    {
        base.OnDeath();
        UpdateUI();
    }

    void UpdateUI()
    {
        healthSlider.maxValue = initialHealth;
        healthSlider.value = health;

        healthText.text = $"{health}/{initialHealth}";
    }
}
