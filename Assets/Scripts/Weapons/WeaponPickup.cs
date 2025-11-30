using System.Collections;
using UnityEngine;

public class WeaponPickup : InteractableBase
{
    [SerializeField] WeaponSO weapon;

    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(weapon)
            spriteRenderer.sprite = weapon.sprite;
    }
    protected override void Start()
    {
        base.Start();
    }
    public void SetWeapon(WeaponSO newWeapon)
    {
        weapon = newWeapon;
        spriteRenderer.sprite = newWeapon.sprite;
    }
    public override void OnInteract(GameObject origin)
    {
        if (!canInteract) return;

        base.OnInteract(origin);

        PlayerWeaponInventory playerWeaponInventory = origin.GetComponent<PlayerWeaponInventory>();

        if (!playerWeaponInventory) return;

        playerWeaponInventory.TryAddWeapon(weapon);
        Destroy(gameObject);
    }
}
