using UnityEngine;

public enum DoorDirection
{
    Left,
    Right, 
    Top,
    Bottom,
    None
}

public class Door : MonoBehaviour
{
    [SerializeField] DoorDirection direction;
    bool isConnected = false;
    Collider2D _collider;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Open();
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
    }

    public void Close()
    {
        _collider.enabled = true;
        spriteRenderer.enabled = true;
    }
}
