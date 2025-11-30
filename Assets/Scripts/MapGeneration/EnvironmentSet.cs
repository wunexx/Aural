using System.Collections.Generic;
using UnityEngine;

public enum EnvironmentType
{
    RootPath,
    DuneTemple,
    TheFinal
}

[CreateAssetMenu(fileName = "EnvironmentSet", menuName = "EnvironmentSets/EnvironmentSet")]
public class EnvironmentSet : ScriptableObject
{
    public EnvironmentType environmentType;

    [Header("Audio")]
    public AudioClip bgMusic;
    public float musicVolume;

    [Header("Camera Color")]
    public Color bgColor;

    [Header("Rooms")]
    public List<Room> startRooms;
    public List<Room> enemyRooms;
    public List<Room> chestRooms;
    public List<Room> specialRooms;
    public List<Room> bossRooms;

    public List<Room> GetRoomListByType(RoomType type)
    {
        return type switch
        {
            RoomType.Start => startRooms,
            RoomType.Enemy => enemyRooms,
            RoomType.Chest => chestRooms,
            RoomType.Special => specialRooms,
            RoomType.Boss => bossRooms,
            _ => startRooms
        };
    }
}
