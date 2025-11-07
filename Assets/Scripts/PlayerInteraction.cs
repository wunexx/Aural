using UnityEngine;
public interface IInteractable
{
    public void OnInteract(GameObject origin);
}
public class PlayerInteraction : MonoBehaviour, IUpdatable
{
    [Header("Interaction")]
    [SerializeField] float interactionRadius;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] float interactionCooldown;

    [Header("References")]
    [SerializeField] UpdateManager updateManager;
    [SerializeField] Material outlineMaterial;

    Material originalMaterial;
    SpriteRenderer currentTarget;

    float interactionTimer;

    private void OnEnable() => updateManager.AddUpdatable(this);
    private void OnDisable() => updateManager.RemoveUpdatable(this);
    private void Start()
    {
        interactionTimer = interactionCooldown;
    }
    public void OnUpdate()
    {
        if (interactionTimer > 0)
            interactionTimer -= Time.deltaTime;

        UpdateTarget();

        if(PlayerInputController.Instance.GetInteractInput() == 1 && interactionTimer <= 0f && currentTarget != null)
        {
            IInteractable interactable = currentTarget.GetComponent<IInteractable>();

            if (interactable == null) return;

            interactable.OnInteract(gameObject);
            interactionTimer = interactionCooldown;
        }
    }
    void UpdateTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionLayer);
        SpriteRenderer newTarget = hit ? hit.GetComponent<SpriteRenderer>() : null;

        if (newTarget == currentTarget) return;

        if (currentTarget != null)
            currentTarget.material = originalMaterial;

        if (newTarget != null)
        {
            originalMaterial = newTarget.material;
            newTarget.material = outlineMaterial;
        }

        currentTarget = newTarget;
    }
}
