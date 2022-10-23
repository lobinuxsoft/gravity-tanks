using UnityEditor;
using CryingOnionTools.ScriptableVariables.Editor;

namespace HNW.Utils
{
    [CustomEditor(typeof(ScoreBoardData))]
    public class ScoreBoardDataEditor : ScriptableVariableEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawInspector((ScoreBoardData)target);
        }
    }
}