using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingSurface : MonoBehaviour
{
    [SerializeField, Min(1)] int width = 1, height = 1;

    [SerializeField, Min(1)] float cellSize = 1;

    [SerializeField] bool drawGizmos = false;

    List<Tilemap> tilemaps = new List<Tilemap>();

    bool[,] walkable;
    private void Start()
    {
        walkable = new bool[width, height];
        UpdateGrid();
    }

    public void UpdateGrid()
    {
        if (walkable == null || walkable.GetLength(0) != width || walkable.GetLength(1) != height)
            walkable = new bool[width, height];

        for (int x = 0; x < walkable.GetLength(0); x++)
        {
            for (int y = 0; y < walkable.GetLength(1); y++)
            {
                walkable[x, y] = IsCellWalkable(GetCellWorldOrigin(x, y));
            }
        }
    }

    public void ClearTilemaps()
    {
        tilemaps.Clear();
        walkable = new bool[width, height];
    }

    bool IsCellWalkable(Vector2 worldPos)
    {
        foreach (var tilemap in tilemaps)
        {
            Vector3Int tilePos = tilemap.WorldToCell(worldPos);
            if (tilemap.HasTile(tilePos))
                return false;
        }
        return true;
    }

    public void SetPosAndSize(Vector2 pos, Vector2 worldSize)
    {
        transform.position = pos;

        width = Mathf.CeilToInt(worldSize.x / cellSize);
        height = Mathf.CeilToInt(worldSize.y / cellSize);

        walkable = new bool[width, height];
    }

    public void AddTilemap(Tilemap tilemap)
    {
        if (tilemap != null && !tilemaps.Contains(tilemap))
            tilemaps.Add(tilemap);
        else
            Debug.Log("Oops! Something wrong with tm assignment!");
    }

    public Vector2 GetCellWorldOrigin(int gridX, int gridY)
    {
        Vector2 origin = (Vector2)transform.position - new Vector2(width, height) * 0.5f * cellSize;
        return origin + new Vector2(gridX * cellSize, gridY * cellSize);
    }

    public Vector2 GetCellWorldCenter(int gridX, int gridY)
    {
        return GetCellWorldOrigin(gridX, gridY) + Vector2.one * cellSize * 0.5f;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        walkable = new bool[width, height];
    }
    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        for (int x = 0; x < walkable.GetLength(0); x++)
        {
            for (int y = 0; y < walkable.GetLength(1); y++)
            {
                Vector3 pos = GetCellWorldCenter(x, y);

                Gizmos.color = walkable[x, y] ? Color.green : Color.red;

                Gizmos.DrawWireCube(pos, Vector3.one * cellSize);
            }
        }
    }
#endif
}
