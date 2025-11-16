using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] float pickupCooldown = 0.15f;
    [SerializeField] WeaponSO weapon;

    SpriteRenderer spriteRenderer;

    bool canInteract = false;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(weapon)
            spriteRenderer.sprite = weapon.sprite;
    }
    void Start()
    {
        StartCoroutine(UpdateCanInteract());
    }
    IEnumerator UpdateCanInteract()
    {
        yield return new WaitForSeconds(pickupCooldown);

        canInteract = true;
    }
    public void SetWeapon(WeaponSO newWeapon)
    {
        weapon = newWeapon;
        spriteRenderer.sprite = newWeapon.sprite;
    }
    public void OnInteract(GameObject origin)
    {
        if (!canInteract) return;

        PlayerWeaponInventory playerWeaponInventory = origin.GetComponent<PlayerWeaponInventory>();

        if (!playerWeaponInventory) return;

        playerWeaponInventory.TryAddWeapon(weapon);
        Destroy(gameObject);
    }
}
