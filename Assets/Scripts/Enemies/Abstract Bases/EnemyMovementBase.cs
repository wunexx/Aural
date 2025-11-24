using UnityEngine;

[RequireComponent(typeof(EnemyBrainBase))]
[RequireComponent(typeof(PathfindingAgent))]
public abstract class EnemyMovementBase : MonoBehaviour
{
    [SerializeField] protected float enemyStopDistance;

    protected PathfindingAgent pathfindingAgent;
    protected UpdateManager updateManager;
    protected EnemyBrainBase brain;

    protected virtual void Awake()
    {
        pathfindingAgent = GetComponent<PathfindingAgent>();
        brain = GetComponent<EnemyBrainBase>();
    }

    public virtual void Move(Vector2 pos)
    {
        float distance = Vector2.Distance(transform.position, pos);

        if (distance > enemyStopDistance)
        {
            if (!pathfindingAgent.HasPath())
            {
                pathfindingAgent.SetDestination(pos);
            }
        }
        else
        {
            pathfindingAgent.ClearPath();
        }
    }
}
 