using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("Appearance")]
    public Sprite sprite;

    [Header("Shooting")]
    public float damage = 10f;
    public float cooldown = 0.5f;
    public float scatter = 5f;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 3f;
}
