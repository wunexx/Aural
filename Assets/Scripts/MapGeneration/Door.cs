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

    [Header("Sounds")]
    [SerializeField] AudioClip doorSound;
    [SerializeField] float volume = 0.2f;

    bool isConnected = false;
    Collider2D _collider;
    SpriteRenderer spriteRenderer;
    Animator animator;


    PathfindingSurface pathfindingSurface;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Open(false);
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

    public void Open(bool playSound = true)
    {
        _collider.enabled = false;
        //spriteRenderer.enabled = false;
        pathfindingSurface.UpdateObstacle(gameObject, true);
        animator.SetTrigger("Open");

        if (playSound)
            SoundManager.Instance.PlayOtherSFX(doorSound, volume);
    }

    public void Close(bool playSound = true)
    {
        _collider.enabled = true;
        //spriteRenderer.enabled = true;
        pathfindingSurface.UpdateObstacle(gameObject, false);
        animator.SetTrigger("Close");

        if(playSound)
            SoundManager.Instance.PlayOtherSFX(doorSound, volume);
    }
}
