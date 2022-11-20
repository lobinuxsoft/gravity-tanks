using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
#endif

namespace HNW
{
    public class EnemyFactory : MonoBehaviour
    {
        static EnemyFactory instance;
        public static EnemyFactory Instance => instance;

        Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

        [SerializeField] LongVariable killEnemiesAmount;
        [SerializeField] EnemyFactoryData factoryData;
        [SerializeField] ExpDropManager expDropManager;

        public static event Action<int> onMaxEnemyChange;
        public static event Action<int> onEnemyRemainingAliveChange;
        public static event Action onAllWavesEnd;
        public static event Action<int> onWaveEnd;
        public static event Action<int> onNextWave;

        WaveModel currentWave;
        int currentWaveNumber;

        int enemiesRemainingToSpawn;
        int enemiesRemainingAlive;

        float nextSpawnTime;

        public int EnemiesRemainingAlive
        {
            get => enemiesRemainingAlive;
            set
            {
                enemiesRemainingAlive = value;
                onEnemyRemainingAliveChange?.Invoke(enemiesRemainingAlive);
            }
        }

        private void Awake()
        {
            instance = this;
            SetupEnemies();
        }

        private void SetupEnemies()
        {
            for (int i = 0; i < factoryData.Waves.Length; i++)
            {
                for (int j = 0; j < factoryData.Waves[i].Enemies.Length; j++)
                {
                    if (!pools.ContainsKey(factoryData.Waves[i].Enemies[j].name))
                    {
                        ObjectPool pool = new GameObject(factoryData.Waves[i].Enemies[j].name).AddComponent<ObjectPool>();
                        pool.transform.SetParent(this.transform);
                        pool.SetObjectToPool(factoryData.Waves[i].Enemies[j]);
                        pools.Add(factoryData.Waves[i].Enemies[j].name, pool);
                    }
                }
            }
        }

        private void Start() => NextWave();

        private void Update()
        {
            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.TimeBetweenSpawns;

                StartCoroutine(SpawnEnemy());
            }
        }

        IEnumerator SpawnEnemy()
        {
            yield return new WaitForEndOfFrame();

            Vector3 pos = MapGenerator.Instance.GetRandomPos();
            GameObject spawnedEnemy = pools[currentWave.Enemies[UnityEngine.Random.Range(0, currentWave.Enemies.Length)].name].GetFromPool();
            spawnedEnemy.transform.position = pos;

            if (spawnedEnemy.TryGetComponent(out EnemyDamageControl damageable))
                damageable.onDie.AddListener(OnEnemyDeath);

            yield return null;
        }

        void OnEnemyDeath(GameObject spawnedEnemy)
        {
            EnemiesRemainingAlive--;
            expDropManager.DropExpInPlace(spawnedEnemy.transform.position);

            if (spawnedEnemy.TryGetComponent(out EnemyDamageControl damageable))
            {
                damageable.onDie.RemoveListener(OnEnemyDeath);
                damageable.FullHeal();
            }

            pools[spawnedEnemy.name].ReturnToPool(spawnedEnemy);

            if (EnemiesRemainingAlive == 0)
                WaveEnd();

            killEnemiesAmount.Value++;

#if UNITY_ANDROID
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_net_guardian, 1, (bool success) => { });
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_seek_and_destroy, 1, (bool success) => { });
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_headhunter, 1, (bool success) => { });
#endif
        }

        void WaveEnd()
        {
            expDropManager.GrabAllActiveExpDrop();

            if (currentWaveNumber < factoryData.Waves.Length)
                onWaveEnd?.Invoke(currentWaveNumber);
            else
                onAllWavesEnd?.Invoke();
        }

        public void NextWave()
        {
            currentWaveNumber++;

            currentWave = factoryData.Waves[(currentWaveNumber - 1) % factoryData.Waves.Length];

            enemiesRemainingToSpawn = currentWave.EnemyCount;
            EnemiesRemainingAlive = enemiesRemainingToSpawn;

            onMaxEnemyChange?.Invoke(currentWave.EnemyCount);

            onNextWave?.Invoke(currentWaveNumber);
        }
    }
}