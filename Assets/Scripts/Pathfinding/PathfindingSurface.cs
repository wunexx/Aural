using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IObstacle
{
    public void Init(PathfindingSurface _pathfindingSurface);
}

public class PathfindingSurface : MonoBehaviour
{
    [SerializeField, Min(1)] int width = 1, height = 1;

    [SerializeField, Min(1)] float cellSize = 1;

    [SerializeField] bool drawGizmos = false;

    List<Collider2D> obstacles = new List<Collider2D>();
    List<Tilemap> tilemaps = new List<Tilemap>();


    bool[,] tilemapGrid; //true = walkable, false = not walkable. Goes for all 3 arrays
    bool[,] obstacleGrid;
    bool[,] finalGrid;

    public bool[,] GetGrid() => finalGrid;

    public bool IsCellWalkable(Vector2Int cell)
    {
        return finalGrid[cell.x, cell.y];
    }

    public List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();


        List<Vector2Int> dirs = new List<Vector2Int>
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),

            new Vector2Int(1, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1),
            new Vector2Int(1, -1)
        };

        foreach (var dir in dirs)
        {
            Vector2Int newPos = pos + dir;

            if (newPos.x < 0 || newPos.y < 0 ||
                newPos.x >= finalGrid.GetLength(0) || newPos.y >= finalGrid.GetLength(1))
                continue;

            if (dir.x != 0 && dir.y != 0)
            {
                Vector2Int cell1 = new Vector2Int(pos.x + dir.x, pos.y);
                Vector2Int cell2 = new Vector2Int(pos.x, pos.y + dir.y);

                if (!IsCellWalkable(cell1) || !IsCellWalkable(cell2))
                    continue;
            }

            neighbors.Add(newPos);
        }

        return neighbors;
    }

    public void MergeGrids()
    {
        finalGrid = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                finalGrid[x, y] = tilemapGrid[x, y] && obstacleGrid[x, y];
            }
        }
    }

    public void UpdateTilemapGrid()
    {
        CreateTilemapGrid();

        for (int x = 0; x < tilemapGrid.GetLength(0); x++)
        {
            for (int y = 0; y < tilemapGrid.GetLength(1); y++)
            {
                tilemapGrid[x, y] = DoesCellContainTile(GetCellWorldOrigin(x, y));
            }
        }

        //Debug.Log(tilemapGrid);
        MergeGrids();
    }

    public void UpdateFullObstacleGrid()
    {
        CreateObstacleGrid();

        foreach (var obstacle in obstacles)
        {
            UpdateObstacleGrid(obstacle, true);
        }

        //Debug.Log(obstacleGrid);
        MergeGrids();
    }

    public void ClearTilemaps()
    {
        tilemaps.Clear();
        CreateTilemapGrid();
    }

    public void ClearObstacles()
    {
        obstacles.Clear();
        CreateObstacleGrid();
    }

    void CreateGrid()
    {
        CreateTilemapGrid();
        CreateObstacleGrid();

        MergeGrids();
    }
    void CreateTilemapGrid()
    {
        tilemapGrid = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemapGrid[x, y] = true;
            }
        }
    }
    void CreateObstacleGrid()
    {
        obstacleGrid = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                obstacleGrid[x, y] = true;
            }
        }
    }


    bool DoesCellContainTile(Vector2 worldPos)
    {
        foreach (var tilemap in tilemaps)
        {
            Vector3Int tilePos = tilemap.WorldToCell(worldPos);
            if (tilemap.HasTile(tilePos))
                return false;
        }
        return true;
    }

    public void UpdateObstacle(GameObject obstacle, bool state)
    {
        Collider2D col = obstacle.GetComponent<Collider2D>();

        if (state && !obstacles.Contains(col))
            obstacles.Add(col);
        else if (!state && obstacles.Contains(col))
            obstacles.Remove(col);

        UpdateObstacleGrid(col, state);
    }

    void UpdateObstacleGrid(Collider2D collider, bool state)
    {
        Vector2Int size = GetObstacleSizeInCells(collider.bounds.size);

        if (!TryGetWorldCell(collider.bounds.center, out Vector2Int cell))
            return;

        int startX = cell.x - size.x / 2;
        int startY = cell.y - size.y / 2;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int gx = startX + x;
                int gy = startY + y;

                if (gx < 0 || gy < 0 || gx >= width || gy >= height)
                    continue;

                obstacleGrid[gx, gy] = state;
            }
        }

        MergeGrids();
    }

    Vector2Int GetObstacleSizeInCells(Vector2 obstacleSize)
    {
        return new Vector2Int(Mathf.CeilToInt(obstacleSize.x / cellSize), Mathf.CeilToInt(obstacleSize.y / cellSize));
    }

    public bool TryGetWorldCell(Vector2 worldPos, out Vector2Int cell)
    {
        Vector2 local = worldPos - GetGridOrigin();

        int gx = Mathf.FloorToInt(local.x / cellSize);
        int gy = Mathf.FloorToInt(local.y / cellSize);

        bool inside = gx >= 0 && gy >= 0 && gx < width && gy < height;

        cell = new Vector2Int(gx, gy);
        return inside;
    }

    public void SetPosAndSize(Vector2 pos, Vector2 worldSize)
    {
        transform.position = pos;

        width = Mathf.CeilToInt(worldSize.x / cellSize);
        height = Mathf.CeilToInt(worldSize.y / cellSize);

        CreateGrid();
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
        return GetGridOrigin() + new Vector2(gridX * cellSize, gridY * cellSize);
    }

    public Vector2 GetGridOrigin()
    {
        return (Vector2)transform.position - new Vector2(width, height) * 0.5f * cellSize;
    }

    public Vector2 GetCellWorldCenter(int gridX, int gridY)
    {
        return GetCellWorldOrigin(gridX, gridY) + Vector2.one * cellSize * 0.5f;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!drawGizmos || !Application.isPlaying) return;

        for (int x = 0; x < finalGrid.GetLength(0); x++)
        {
            for (int y = 0; y < finalGrid.GetLength(1); y++)
            {
                Vector3 pos = GetCellWorldCenter(x, y);

                Gizmos.color = finalGrid[x, y] ? Color.green : Color.red;

                Gizmos.DrawWireCube(pos, Vector3.one * cellSize * 0.99f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(GetCellWorldCenter(x, y), 0.1f);
            }
        }
    }
#endif
}
