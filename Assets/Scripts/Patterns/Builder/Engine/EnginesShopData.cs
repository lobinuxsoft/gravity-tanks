using System.Collections.Generic;
using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Engine Builder/Engines Shop Data")]
    public class EnginesShopData : ScriptableObject
    {
        [SerializeField] List<EngineData> enginesDatas = new List<EngineData>();

        public EngineData GetEngineDataByName(string name) => enginesDatas.Find(x => x.name == name);

        public List<EngineData> GetAllEngines() => enginesDatas;
    }
}