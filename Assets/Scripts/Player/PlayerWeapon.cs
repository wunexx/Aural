using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour, IUpdatable
{
    [Header("References")]
    [SerializeField] UpdateManager updateManager;
    [SerializeField] SpriteRenderer weaponSpriteRenderer;
    [SerializeField] Transform weaponRotPivot;
    [SerializeField] Transform firepoint;

    SpriteRenderer spriteRenderer;
    WeaponSO currentWeapon;

    float shootTimer;
    private void OnEnable() => updateManager.AddUpdatable(this);

    private void OnDisable() => updateManager.RemoveUpdatable(this);
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ChangeWeapon(WeaponSO newWeapon)
    {
        if (newWeapon == null) return;

        currentWeapon = newWeapon;
        weaponSpriteRenderer.sprite = newWeapon.sprite;

        shootTimer = newWeapon.cooldown;
    }

    public void OnUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 dir = mousePosition - (Vector2)transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (mousePosition.x >= transform.position.x)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            spriteRenderer.flipX = false;
            weaponRotPivot.localScale = new Vector3(1, 1, 1);
        }
        else if (mousePosition.x < transform.position.x)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            spriteRenderer.flipX = true;
            weaponRotPivot.localScale = new Vector3(1, -1, 1);
            //angle += 180;

        }

        weaponRotPivot.rotation = Quaternion.Euler(0, 0, angle);

        if (currentWeapon == null) return;

        if (shootTimer > 0)
            shootTimer -= Time.deltaTime;

        if(PlayerInputController.Instance.GetShootInput() == 1 && shootTimer <= 0)
        {
            Shoot();
            shootTimer = currentWeapon.cooldown;
        }
    }
    void Shoot()
    {
        for (int i = 0; i < currentWeapon.projectilesPerShot; i++)
        {
            float scatterAngle = Random.Range(-currentWeapon.scatter, currentWeapon.scatter);
            Vector2 scatterDir = (Quaternion.Euler(0, 0, scatterAngle) * firepoint.right).normalized;

            float angle = Mathf.Atan2(scatterDir.y, scatterDir.x) * Mathf.Rad2Deg;

            ProjectileBase projectile = Instantiate(currentWeapon.projectilePrefab, firepoint.position, Quaternion.Euler(0, 0, angle)).GetComponent<ProjectileBase>();

            projectile.Init(scatterDir, updateManager, currentWeapon.projectileSpeed, currentWeapon.projectileLifetime, currentWeapon.damage);
        }
    }
}
