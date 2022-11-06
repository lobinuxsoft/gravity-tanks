using HNW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class Spawner : MonoBehaviour
{
    Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();
    //ObjectPool enemyPool;

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
            damageable.onDie.AddListener(() => OnEnemyDeath(spawnedEnemy));

        yield return null;
    }

    void OnEnemyDeath(GameObject spawnedEnemy)
    {
        enemiesRemainingAlive--;

        if (spawnedEnemy.TryGetComponent(out EnemyDamageControl damageable))
        {
            damageable.onDie.RemoveAllListeners();
            damageable.FullHeal();
        }

        pools[spawnedEnemy.name].ReturnToPool(spawnedEnemy);

        if (enemiesRemainingAlive == 0)
            NextWave();
    }

    IEnumerator ResetPlayerPosition()
    {
        yield return new WaitForEndOfFrame();
        player.gameObject.SetActive(false);

        if(player.TryGetComponent(out Rigidbody body))
        {
            body.isKinematic = true;
            body.velocity -= body.velocity;
            body.isKinematic = false;
        }

        player.position = MapGenerator.Instance.GetRandomPos() + Vector3.up * .5f;
        player.rotation = Quaternion.identity;

        player.gameObject.SetActive(true);
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

            StartCoroutine(ResetPlayerPosition());
        }
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