using GravityTanks;
using GravityTanks.Enemy;
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

    private void Awake() => enemyPool = GetComponent<ObjectPool>();

    private void Start() => NextWave();

    private void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.TimeBetweenSpawns;

            GameObject spawnedEnemy = enemyPool.GetFromPool();
            spawnedEnemy.transform.position = Vector3.zero;

            if(spawnedEnemy.TryGetComponent(out Damageable damageable))
                damageable.onDie.AddListener(() => OnEnemyDeath(spawnedEnemy));
        }
    }

    void OnEnemyDeath(GameObject spawnedEnemy)
    {
        enemiesRemainingAlive--;
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