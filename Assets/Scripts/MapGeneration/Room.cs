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

    [SerializeField] List<EnemyBrainBase> enemies;

    bool isCleared = false;
    private void Start()
    {

        isCleared = false;
    }

    List<PathfindingAgent> GetPathfindingAgents()
    {
        List<PathfindingAgent> agents = new List<PathfindingAgent>();

        foreach (var enemy in enemies)
        {
            PathfindingAgent pa = enemy.GetComponent<PathfindingAgent>();

            if (pa != null)
                agents.Add(pa);
        }

        return agents;
    }

    public void InitAgents(PathfindingSurface ps, UpdateManager um)
    {
        List<PathfindingAgent> agents = GetPathfindingAgents();

        foreach (var agent in agents)
            agent.Init(ps, um);

        foreach (var enemy in enemies)
            enemy.Init(um, this);
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
        if (collision.CompareTag("Player") && isCleared == false && roomType != RoomType.Start && enemies.Count > 0)
        {
            CloseAllDoors();
            EnableEnemies(collision.gameObject);
        }
    }
    void EnableEnemies(GameObject target)
    {
        foreach (var enemy in enemies)
            enemy.InitTarget(target);
    }
    void CloseAllDoors()
    {
        foreach (var door in doors)
            door.Close();
    }
    void OpenAllDoors() 
    {
        foreach (var door in doors)
            door.Open();
    }
    public void OnEnemyKilled(EnemyBrainBase enemy)
    {
        if (enemies.Contains(enemy))
            enemies.Remove(enemy);

        if(enemies.Count <= 0)
        {
            OpenAllDoors();
            isCleared = true;
        }
    }
}
