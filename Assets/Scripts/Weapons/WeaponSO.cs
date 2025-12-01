using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("Appearance")]
    public string _name;
    public Sprite sprite;

    [Header("Shooting")]
    public float damage = 10f;
    public float cooldown = 0.5f;
    public float scatter = 5f;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 3f;
    public int projectilesPerShot = 1;

    [Header("Sounds")]
    public AudioClip[] shootSounds;
    public float volume = 0.2f;

}
