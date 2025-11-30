using UnityEngine;

public class WeaponCrate : InteractableBase
{
    [Header("Drops")]
    [SerializeField] GameObject[] possibleDrops;

    [SerializeField] WeaponSO[] possibleWeapons;

    [Header("Effect")]
    [SerializeField] GameObject effectPrefab;
    [SerializeField] float effectDestroyTime;

    [Header("Sounds")]
    [SerializeField] AudioClip sound;
    [SerializeField] float volume = 0.2f;

    public override void OnInteract(GameObject origin)
    {
        if (!canInteract) return;

        base.OnInteract(origin);

        GameObject drop = possibleDrops[Random.Range(0, possibleDrops.Length)];

        GameObject obj = Instantiate(drop, transform.position, Quaternion.identity);

        WeaponPickup pickup = obj.GetComponent<WeaponPickup>();

        if (pickup != null)
            pickup.SetWeapon(possibleWeapons[Random.Range(0, possibleWeapons.Length)]);

        if(effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

            Destroy(effect, effectDestroyTime);
        }

        if(sound != null)
            SoundManager.Instance.PlayOtherSFX(sound, volume);

        Destroy(gameObject);
    }
}
