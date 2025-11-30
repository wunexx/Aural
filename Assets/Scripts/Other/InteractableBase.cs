using UnityEngine;
using System.Collections;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected float pickupCooldown = 0.15f;

    protected bool canInteract = false;

    protected virtual void Start()
    {
        StartCoroutine(UpdateCanInteract());
    }
    protected virtual IEnumerator UpdateCanInteract()
    {
        yield return new WaitForSeconds(pickupCooldown);

        canInteract = true;
    }

    public virtual void OnInteract(GameObject origin)
    {
    }
}
