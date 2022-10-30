using UnityEngine;
using CryingOnionTools.ScriptableVariables;
using System.Collections.Generic;

namespace HNW.Utils
{
    [CreateAssetMenu(menuName = "Gravity Tanks/ Utils/ Score Board Data")]
    public class ScoreBoardData : ScriptableVariable<List<ScoreData>>
    {
        public override void EraseData()
        {
            base.EraseData();
            value.Clear();
        }
    }
}