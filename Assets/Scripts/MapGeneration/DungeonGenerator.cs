using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] DungeonFlow dungeonFlow;
    [SerializeField] EnvironmentSet[] environmentSets;

    [SerializeField] Vector2 roomSize;

    [SerializeField] PathfindingSurface pathfindingSurface;

    [SerializeField] UpdateManager updateManager;

    Dictionary<Vector2, Room> createdRooms;
    List<Vector2> placementOrder;

    private void Start()
    {
        GenerateDungeon(EnvironmentType.Environment1);
    }

    public void GenerateFuncForButton() //only for testing
    {
        GenerateDungeon(EnvironmentType.Environment1);
    }

    public void GenerateDungeon(EnvironmentType environmentType)
    {
        CleanDungeon();
        pathfindingSurface.ClearTilemaps();
        pathfindingSurface.ClearObstacles();

        createdRooms = new Dictionary<Vector2, Room>();
        placementOrder = new List<Vector2>();

        EnvironmentSet environmentSet = GetEnvironmentSetByType(environmentType);

        if(environmentSet == null)
        {
            Debug.LogWarning("Environment Set not found !!!");
            return;
        }

        for (int i = 0; i < dungeonFlow.steps.Count; i++)
        {
            for (int j = 0; j < dungeonFlow.steps[i].count; j++)
            {
                if(createdRooms.Count <= 0)
                {
                    GameObject roomObj = GetRandomRoom(environmentSet.GetRoomListByType(RoomType.Start)).gameObject;
                    Room room = Instantiate(roomObj, transform.position, Quaternion.identity).GetComponent<Room>();

                    AddRoomToPathfindingSurface(room);
                    room.InitAgents(pathfindingSurface, updateManager);

                    createdRooms.Add(transform.position, room);
                    placementOrder.Add(transform.position);
                }
                else
                {
                    Vector2 lastPos = placementOrder[placementOrder.Count - 1];

                    Room lastRoom = createdRooms[lastPos];

                    if(lastRoom.HasUnconnectedDoors())
                    {
                        DoorDirection dir = lastRoom.GetFirstUnconnectedDoor();

                        DoorDirection oppDir = GetOppositeDirection(dir);

                        List<Room> roomTypes = environmentSet.GetRoomListByType(dungeonFlow.steps[i].roomType);

                        Vector2 pos = lastPos + GetVectorByDoorDirection(dir) * roomSize;

                        List<Room> validPrefabs = new List<Room>();
                        foreach (Room prefab in roomTypes)
                        {
                            if (prefab.HasDoor(oppDir) && RoomFitsHere(prefab, pos))
                                validPrefabs.Add(prefab);
                        }

                        if (validPrefabs.Count == 0)
                        {
                            Debug.LogWarning("No valid room fits at " + pos);
                            return;
                        }
                        Room roomToPlace = GetRandomRoom(validPrefabs);

                        if (createdRooms.ContainsKey(pos))
                        {
                            Debug.LogError("Overlapping room attempted at " + pos);
                            continue;
                        }

                        Room room = Instantiate(roomToPlace.gameObject, pos, Quaternion.identity).GetComponent<Room>();

                        AddRoomToPathfindingSurface(room);
                        room.InitAgents(pathfindingSurface, updateManager);

                        lastRoom.MarkDoorAsConnected(dir);
                        room.MarkDoorAsConnected(oppDir);

                        createdRooms.Add(pos, room);
                        placementOrder.Add(pos);
                    }
                }
            }
        }
        SetupPathfindingSurface();
        pathfindingSurface.UpdateTilemapGrid();
    }

    public void SetupPathfindingSurface()
    {
        if (createdRooms.Count <= 0)
            return;

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (var kvp in createdRooms)
        {
            Vector2 pos = kvp.Key;

            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;
        }

        Vector2 size = new Vector2(maxX - minX, maxY - minY) + roomSize;
        Vector2 center = new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);

        pathfindingSurface.SetPosAndSize(center, size);
    }

    bool RoomFitsHere(Room roomPrefab, Vector2 pos)
    {
        foreach (var door in roomPrefab.doors)
        {
            DoorDirection dir = door.GetDoorDirection();
            if (dir == DoorDirection.None)
                continue;

            Vector2 neighborPos = pos + GetVectorByDoorDirection(dir) * roomSize;

            if (createdRooms.TryGetValue(neighborPos, out Room neighborRoom))
            {
                if (!neighborRoom.HasDoor(GetOppositeDirection(dir)))
                    return false;
            }
        }

        return true;
    }

    void AddRoomToPathfindingSurface(Room room)
    {
        Tilemap tilemap = room.transform.Find("Walls").GetComponent<Tilemap>();

        if (tilemap != null)
            pathfindingSurface.AddTilemap(tilemap);

        foreach(var door in room.doors)
        {
            door.Init(pathfindingSurface);
        }
    }
    public Room GetRandomRoomByDir(List<Room> rooms, DoorDirection dir)
    {
        List<Room> roomsWithDir = new List<Room>();

        foreach (var room in rooms)
        {
            if (room.HasDoor(dir))
                roomsWithDir.Add(room);
        }

        if (roomsWithDir.Count == 0)
            return null;

        return GetRandomRoom(roomsWithDir);
    }


    public void CleanDungeon()
    {
        if (createdRooms == null) return;

        foreach (var kvp in createdRooms)
        {
            if (kvp.Value != null)
                Destroy(kvp.Value.gameObject);
        }

        createdRooms.Clear();
        placementOrder.Clear();
    }

    Room GetRandomRoom(List<Room> rooms)
    {
        return rooms[Random.Range(0, rooms.Count)];
    } 

    public EnvironmentSet GetEnvironmentSetByType(EnvironmentType type)
    {
        foreach (var set in environmentSets)
        {
            if (set.environmentType == type) return set;
        }
        return null;
    }

    public DoorDirection GetOppositeDirection(DoorDirection dir) 
    {
        return dir switch
        {
            DoorDirection.Top => DoorDirection.Bottom,
            DoorDirection.Bottom => DoorDirection.Top,
            DoorDirection.Left => DoorDirection.Right,
            DoorDirection.Right => DoorDirection.Left,
            _ => dir
        };
    }

    public Vector2 GetVectorByDoorDirection(DoorDirection dir)
    {
        return dir switch
        {
            DoorDirection.Top => Vector2.up,
            DoorDirection.Bottom => Vector2.down,
            DoorDirection.Right => Vector2.right,
            DoorDirection.Left => Vector2.left,
            _ => Vector2.zero
        };
    }
}
