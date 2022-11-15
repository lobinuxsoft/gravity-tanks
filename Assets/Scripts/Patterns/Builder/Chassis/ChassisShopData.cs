using System.Collections.Generic;
using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Chassis Builder/Chassis Shop Data")]
    public class ChassisShopData : ScriptableObject
    {
        [SerializeField] List<ChassisData> chassisDatas = new List<ChassisData>();

        public ChassisData GetChassisDataByName(string name) => chassisDatas.Find(x => x.name == name);

        public List<ChassisData> GetAllChassis() => chassisDatas;
    }
}