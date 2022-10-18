using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator map = (MapGenerator)target;

        if(GUILayout.Button("Regenerate Map"))
        {
            map.GenerateMap();
        }
    }
}
