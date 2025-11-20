using System;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Start,
    Enemy,
    Chest,
    Special,
    Boss
}
public class Room : MonoBehaviour
{
    public Door[] doors;
    [SerializeField] RoomType roomType;

    [SerializeField] PathfindingAgent[] agents;

    //add array with all enemies, when there will be any xD

    bool isCleared = false;
    private void Start()
    {
        isCleared = false;
    }

    public void InitAgents(PathfindingSurface ps, UpdateManager um)
    {
        foreach (var agent in agents)
        {
            agent.Init(ps, um);
        }
    }

    public bool HasDoor(DoorDirection dir)
    {
        foreach (var door in doors)
        {
            if (door.GetDoorDirection() == dir)
                return true;
        }
        return false;
    }

    public void MarkDoorAsConnected(DoorDirection dir)
    {
        foreach (var door in doors)
        {
            if (door.GetDoorDirection() == dir)
            {
                door.MarkIsConnected(true);
                return;
            }
        }
    }

    public DoorDirection GetFirstUnconnectedDoor()
    {
        foreach (var door in doors)
        {
            if (door.GetIsConnected() == false)
                return door.GetDoorDirection();
        }
        return DoorDirection.None;
    }

    public bool HasUnconnectedDoors()
    {
        foreach (var door in doors)
        {
            if (door.GetIsConnected() == false)
                return true;
        }
        return false;
    }

    public int GetDoorCount()
    {
        return doors.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isCleared == false)
        {
            CloseAllDoors();
            EnableEnemies();
        }
    }
    void EnableEnemies()
    {
        //enables enemy ais
    }
    void CloseAllDoors()
    {
        foreach (var door in doors)
        {
            door.Close();
        }
    }
    void OnEnemyKilled() //will check if the enemy that was killed is the last one, if it was open all doors and mark room as cleared
    {
        
    }
}
