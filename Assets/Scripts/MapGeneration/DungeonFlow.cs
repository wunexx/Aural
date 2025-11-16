using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonFlowStep
{
    public RoomType roomType;
    public int count;
}

[CreateAssetMenu(fileName = "DungeonFlow", menuName = "DungeonFlows/DungeonFlow")]
public class DungeonFlow : ScriptableObject
{
    public List<DungeonFlowStep> steps;
}
