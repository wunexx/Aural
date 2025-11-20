#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathfindingAgent))]
public class PathfindingAgentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathfindingAgent pa = (PathfindingAgent)target;

        if (GUILayout.Button("Go"))
        {
            pa.SetDestination(pa.testGoal);
        }
    }
}
#endif