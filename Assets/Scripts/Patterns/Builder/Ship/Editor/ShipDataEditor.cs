using CryingOnionTools.ScriptableVariables.Editor;
using UnityEditor;

namespace HNW
{
    [CustomEditor(typeof(ShipData))]
    public class ShipDataEditor : ScriptableVariableEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawInspector((ShipData)target);
        }
    }
}