using GravityTanks;
using GravityTanks.Enemy;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class Spawner : MonoBehaviour
{
    ObjectPool enemyPool;

    [SerializeField] Wave[] waves;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;

    float nextSpawnTime;

    MapGenerator mapGenerator;

    private void Awake()
    {
        enemyPool = GetComponent<ObjectPool>();
        mapGenerator = FindObjectOfType<MapGenerator>();
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
        Vector3 pos = mapGenerator.GetRandomPos();
        GameObject spawnedEnemy = enemyPool.GetFromPool();
        spawnedEnemy.transform.position = pos;

        if (spawnedEnemy.TryGetComponent(out Damageable damageable))
            damageable.onDie.AddListener(() => OnEnemyDeath(spawnedEnemy));

        yield return null;
    }

    void OnEnemyDeath(GameObject spawnedEnemy)
    {
        enemiesRemainingAlive--;

        if (spawnedEnemy.TryGetComponent(out Damageable damageable))
        {
            damageable.onDie.RemoveAllListeners();
            damageable.FullHeal();
        }

            enemyPool.ReturnToPool(spawnedEnemy);

        if (enemiesRemainingAlive == 0)
            NextWave();
    }

    void NextWave()
    {
        currentWaveNumber++;

        if((currentWaveNumber - 1) < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.EnemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }
}

[System.Serializable]
public struct Wave
{
    [SerializeField] int enemyCount;
    [SerializeField] float timeBetweenSpawns;

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
}