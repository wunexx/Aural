using UnityEngine;

public class PlayerWeapon : MonoBehaviour, IUpdatable
{
    [Header("References")]
    [SerializeField] UpdateManager updateManager;
    [SerializeField] SpriteRenderer weaponSpriteRenderer;
    [SerializeField] Transform firepoint;

    WeaponSO currentWeapon;

    private void OnEnable()
    {
        updateManager.AddUpdatable(this);
    }

    private void OnDisable()
    {
        updateManager.RemoveUpdatable(this);
    }
    public void ChangeWeapon(WeaponSO newWeapon)
    {
        if (newWeapon == null) return;

        currentWeapon = newWeapon;
        weaponSpriteRenderer.sprite = newWeapon.sprite;
    }

    public void OnUpdate()
    {

    }
}
