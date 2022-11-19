using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Map Generator/Map Data")]
    public class MapData : ScriptableObject
    {
        [SerializeField] MapStruct[] maps;

        public MapStruct[] Maps => maps;
    }
}