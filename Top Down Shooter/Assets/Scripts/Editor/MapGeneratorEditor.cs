using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Map map = (Map)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Map"))
        {
            map.GenerateMap();
        }
    }
}
