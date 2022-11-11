using CryingOnionTools.ScriptableVariables.Editor;
using UnityEditor;

[CustomEditor(typeof(LongVariable))]
public class LongVariableEditor : ScriptableVariableEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawInspector((LongVariable)target);
    }
}