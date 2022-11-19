using UnityEngine;

namespace HNW
{
    [System.Serializable]
    public struct WaveModel
    {
        [SerializeField] int enemyCount;
        [SerializeField] float timeBetweenSpawns;
        [SerializeField] GameObject[] enemies;

        public int EnemyCount
        {
            get => enemyCount;
            set => enemyCount = value;
        }

        public float TimeBetweenSpawns
        {
            get => timeBetweenSpawns;
            set => timeBetweenSpawns = value;
        }

        public GameObject[] Enemies => enemies;
    }
}