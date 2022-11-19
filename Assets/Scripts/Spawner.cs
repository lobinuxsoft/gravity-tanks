using HNW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
#endif

public class Spawner : MonoBehaviour
{
    static Spawner instance;
    public static Spawner Instance => instance;

    Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

    [SerializeField] LongVariable killEnemiesAmount;
    [SerializeField] Wave[] waves;
    [SerializeField] ExpDropManager expDropManager;

    public static event Action onAllWavesEnd;
    public static event Action<int> onWaveEnd;
    public static event Action<int> onNextWave; 

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;

    float nextSpawnTime;

    private void Awake()
    {
        instance = this;
        SetupEnemies();
    }

    private void SetupEnemies()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            for (int j = 0; j < waves[i].Enemies.Length; j++)
            {
                if(!pools.ContainsKey(waves[i].Enemies[j].name))
                {
                    ObjectPool pool = new GameObject(waves[i].Enemies[j].name).AddComponent<ObjectPool>();
                    pool.transform.SetParent(this.transform);
                    pool.SetObjectToPool(waves[i].Enemies[j]);
                    pools.Add(waves[i].Enemies[j].name, pool);
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
        enemiesRemainingAlive--;
        expDropManager.DropExpInPlace(spawnedEnemy.transform.position);

        if (spawnedEnemy.TryGetComponent(out EnemyDamageControl damageable))
        {
            damageable.onDie.RemoveListener(OnEnemyDeath);
            damageable.FullHeal();
        }

        pools[spawnedEnemy.name].ReturnToPool(spawnedEnemy);

        if (enemiesRemainingAlive == 0)
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

        if (currentWaveNumber >= waves.Length)
            onAllWavesEnd?.Invoke();
        else
            onWaveEnd?.Invoke(currentWaveNumber);
    }

    public void NextWave()
    {
        currentWaveNumber++;

        currentWave = waves[(currentWaveNumber - 1) % waves.Length];

        enemiesRemainingToSpawn = currentWave.EnemyCount;
        enemiesRemainingAlive = enemiesRemainingToSpawn;

        onNextWave?.Invoke(currentWaveNumber);
    }
}

[System.Serializable]
public struct Wave
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