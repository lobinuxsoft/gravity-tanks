using UnityEditor;
using UnityEngine;

namespace HNW
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MapGenerator map = (MapGenerator)target;

            if (DrawDefaultInspector())
                map.GenerateMap();

            if (GUILayout.Button("Generate Map"))
                map.GenerateMap();
        }
    }
}