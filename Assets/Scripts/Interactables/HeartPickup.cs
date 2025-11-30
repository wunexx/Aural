using UnityEngine;

public class HeartPickup : InteractableBase
{
    [SerializeField] float healAmount;

    public override void OnInteract(GameObject origin)
    {
        if (!canInteract) return;

        base.OnInteract(origin);

        HealthBase health = origin.GetComponent<HealthBase>();

        if (health != null)
        {
            //Debug.Log(origin.name);
            health.Heal(healAmount);
        }
        else
        {
            Debug.LogWarning("No HealthBase found on origin!");
        }

        Destroy(gameObject);
    }
}
