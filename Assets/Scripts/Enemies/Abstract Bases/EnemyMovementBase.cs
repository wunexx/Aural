using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(EnemyBrainBase))]
[RequireComponent(typeof(PathfindingAgent))]
public abstract class EnemyMovementBase : MonoBehaviour, IUpdatable
{
    [SerializeField] protected float enemyStopDistance;
    [SerializeField] protected float targetUpdateCooldown = 1f;

    protected float targetUpdateTimer = 0;

    protected PathfindingAgent pathfindingAgent;
    protected UpdateManager updateManager;
    protected EnemyBrainBase brain;

    protected Animator animator;

    protected virtual void OnDisable() { if (updateManager) updateManager.RemoveUpdatable(this); }

    public virtual void Init(UpdateManager um)
    {
        updateManager = um;
        um.AddUpdatable(this);
    }

    protected virtual void Awake()
    {
        pathfindingAgent = GetComponent<PathfindingAgent>();
        brain = GetComponent<EnemyBrainBase>();

        animator = GetComponent<Animator>();
    }

    public virtual void Move(Vector2 pos)
    {
        float distance = Vector2.Distance(transform.position, pos);

        if (animator != null)
            animator.SetBool("movement", pathfindingAgent.IsMoving());
        else
            Debug.Log("Stupid mistake again!");

        //Debug.Log(pathfindingAgent.CalculatedVelocity);

        if (distance > enemyStopDistance)
        {
            if (targetUpdateTimer >= targetUpdateCooldown)
                UpdateDestination(pos);

            if (!pathfindingAgent.HasPath())
                UpdateDestination(pos);
        }
        else
        {
            pathfindingAgent.ClearPath();
        }
    }

    protected virtual void UpdateDestination(Vector2 pos)
    {
        pathfindingAgent.SetDestination(pos);
        targetUpdateTimer = 0f;
    }

    public virtual void OnUpdate()
    {
        if (targetUpdateTimer < targetUpdateCooldown)
            targetUpdateTimer += Time.deltaTime;
    }
}
 