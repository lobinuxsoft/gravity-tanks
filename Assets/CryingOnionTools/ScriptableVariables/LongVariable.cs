using CryingOnionTools.ScriptableVariables;
using UnityEngine;

[CreateAssetMenu(fileName = "New Long Variable", menuName = "Crying Onion Tools/ Scriptable Variables/ Long Variable")]
public class LongVariable : ScriptableVariable<long>
{
    public override void EraseData()
    {
        base.EraseData();
        value = 0;
    }
}