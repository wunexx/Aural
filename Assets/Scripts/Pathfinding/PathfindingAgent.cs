using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PathfindingAgent : MonoBehaviour, IFixedUpdatable
{
    [SerializeField] float moveSpeed;

#if UNITY_EDITOR
    [Header("Test pos for debuging")]
    public Vector2 testGoal;
#endif

    List<Vector2> currentPath = new List<Vector2>();
    int pathIndex;

    PathfindingSurface pathfindingSurface;
    UpdateManager updateManager;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetDestination(Vector2 destination)
    {
        if (!pathfindingSurface)
        {
            Debug.LogWarning("Couldnt create path cuz pathfinding surface is null!");
            return;
        }

        List<Vector2Int> path = GetPath(transform.position, destination);

        currentPath.Clear();
        pathIndex = 0;

        foreach (var pos in path)
            currentPath.Add(pathfindingSurface.GetCellWorldCenter(pos.x, pos.y));
    }

    public void OnFixedUpdate()
    {
        if (currentPath.Count <= 0 || pathIndex >= currentPath.Count)
            return;

        Vector2 target = currentPath[pathIndex];
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(transform.position, target) < 0.05f)
            pathIndex++;
    }

    public void Init(PathfindingSurface ps, UpdateManager um)
    {
        pathfindingSurface = ps;
        updateManager = um;

        updateManager.AddFixedUpdatable(this);
    }

    List<Vector2Int> GetPath(Vector2 start, Vector2 goal)
    {
        if (!pathfindingSurface.TryGetWorldCell(start, out Vector2Int startCell) ||
            !pathfindingSurface.TryGetWorldCell(goal, out Vector2Int goalCell))
        {
            Debug.LogWarning("Conversion from world pos to cell failed!");
            return new List<Vector2Int>();
        }

        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        List<Node> openSet = new List<Node>();

        Node goalNode = null;

        Node startNode = new Node(startCell, null, 0, Heuristic(startCell, goalCell));
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            foreach (var node in openSet)
            {
                if (node.F < currentNode.F || (node.F == currentNode.F && node.H < currentNode.H))
                    currentNode = node;
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode.position);

            if(currentNode.position == goalCell)
            {
                goalNode = currentNode;
                break;
            }

            foreach (var neighbor in pathfindingSurface.GetNeighbors(currentNode.position))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                if (!pathfindingSurface.IsCellWalkable(neighbor))
                    continue;

                int dx = Mathf.Abs(neighbor.x - currentNode.position.x);
                int dy = Mathf.Abs(neighbor.y - currentNode.position.y);

                int moveCost = (dx == 1 && dy == 1) ? 14 : 10;

                int newG = currentNode.G + moveCost;

                Node neighborNode = openSet.Find(n => n.position == neighbor);
                if(neighborNode == null)
                {
                    neighborNode = new Node(neighbor, currentNode, newG, Heuristic(neighbor, goalCell));
                    openSet.Add(neighborNode);
                }
                else if(newG < neighborNode.G)
                {
                    neighborNode.G = newG;
                    neighborNode.parent = currentNode;
                }

            }
        }

        return ReconstructPath(goalNode);

    }

    List<Vector2Int> ReconstructPath(Node node)
    {
        if (node == null)
            return new List<Vector2Int>();

        List<Vector2Int> positions = new List<Vector2Int>();
        Node current = node;

        while(current != null)
        {
            positions.Add(current.position);
            current = current.parent;
        }

        positions.Reverse();

        return positions;
    }

    float Heuristic(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        int diagonal = Mathf.Min(dx, dy);
        int straight = Mathf.Abs(dx - dy);

        return 14 * diagonal + 10 * straight;
    }
}
class Node
{
    public Vector2Int position;
    public Node parent;
    public int G; //move cost
    public float H; //estimated left
    public float F => G + H; //sum

    public Node(Vector2Int pos, Node _parent, int _G, float _H)
    {
        position = pos;
        parent = _parent;
        G = _G;
        H = _H;
    }
}
