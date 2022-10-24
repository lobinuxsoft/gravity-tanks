using HNW;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class Spawner : MonoBehaviour
{
    ObjectPool enemyPool;

    [SerializeField] Transform player;
    [SerializeField] Wave[] waves;

    public event Action<int> OnNextWave; 

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;

    float nextSpawnTime;

    private void Awake()
    {
        enemyPool = GetComponent<ObjectPool>();
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

    void ResetPlayerPosition()
    {
        player.position = MapGenerator.Instance.GetMapCentrePos();
    }

    void NextWave()
    {
        currentWaveNumber++;

        if((currentWaveNumber - 1) < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.EnemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            OnNextWave?.Invoke(currentWaveNumber);

            ResetPlayerPosition();
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