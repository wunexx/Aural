#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathfindingSurface))]
public class PathfindingSurfaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathfindingSurface ps = (PathfindingSurface)target;

        if(GUILayout.Button("Update Grid"))
        {
            ps.UpdateGrid();

            //to save changes
            EditorUtility.SetDirty(ps);
        }
    }
}
#endif