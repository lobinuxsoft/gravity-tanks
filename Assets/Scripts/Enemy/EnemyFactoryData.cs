using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Enemy Factory/Data")]
    public class EnemyFactoryData : ScriptableObject
    {
        [SerializeField] WaveModel[] waves;

        public WaveModel[] Waves => waves;
    }
}