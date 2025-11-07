using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("Appearance")]
    public Sprite sprite;

    [Header("Shooting")]
    public float damage;
    public float cooldown;
    public float scatter;

    [Header("Projectile")]
    public float projectileForce;
    public GameObject projectilePrefab;
}
