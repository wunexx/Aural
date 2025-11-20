using UnityEngine;

public enum DoorDirection
{
    Left,
    Right, 
    Top,
    Bottom,
    None
}

public class Door : MonoBehaviour, IObstacle
{
    [SerializeField] DoorDirection direction;
    bool isConnected = false;
    Collider2D _collider;
    SpriteRenderer spriteRenderer;

    PathfindingSurface pathfindingSurface;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Open();
    }

    public Vector2 GetDoorSize()
    {
        return _collider.bounds.size;
    }

    public void Init(PathfindingSurface _pathfindingSurface)
    {
        pathfindingSurface = _pathfindingSurface;
    }

    public DoorDirection GetDoorDirection()
    {
        return direction;
    }

    public bool GetIsConnected() { return isConnected; }

    public void MarkIsConnected(bool state)
    {
        isConnected = state;
    }

    public void Open()
    {
        _collider.enabled = false;
        spriteRenderer.enabled = false;
        pathfindingSurface.UpdateObstacle(gameObject, true);
    }

    public void Close()
    {
        _collider.enabled = true;
        spriteRenderer.enabled = true;
        pathfindingSurface.UpdateObstacle(gameObject, false);
    }
}
