using UnityEngine;

public class RangeEnemyAttack : EnemyAttackBase
{
    [Header("Shooting")]
    [SerializeField] Transform firepoint;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float scatter = 5;
    [SerializeField] float projectileLifetime = 2;
    [SerializeField] float projectileSpeed;

    public override void OnUpdate()
    {
        if (!HasTarget()) return;

        base.OnUpdate();

        if (CanAttack())
        {
            Shoot();
            OnAttack();
        }
    }

    void Shoot()
    {
        float scatterAngle = Random.Range(-scatter, scatter);
        Vector2 scatterDir = (Quaternion.Euler(0, 0, scatterAngle) * firepoint.right).normalized;

        float angle = Mathf.Atan2(scatterDir.y, scatterDir.x) * Mathf.Rad2Deg;

        ProjectileBase projectileBase = Instantiate(projectilePrefab, firepoint.position, Quaternion.Euler(0, 0, angle)).GetComponent<ProjectileBase>();

        projectileBase.Init(scatterDir, updateManager, projectileSpeed, projectileLifetime, damage);
    }
}
