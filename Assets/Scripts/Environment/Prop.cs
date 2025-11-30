using UnityEngine;

public abstract class Prop : HealthBase
{
    [SerializeField] GameObject[] possibleDrops;
    [SerializeField] float dropChance = 0.05f;

    protected override void OnDeath()
    {
        base.OnDeath();

        if (Random.value < dropChance && possibleDrops.Length > 0)
            Instantiate(possibleDrops[Random.Range(0, possibleDrops.Length)], transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
