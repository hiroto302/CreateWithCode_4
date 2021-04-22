using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    public GameObject enemyPrefab;
    private float _spawnRange = 9;

    public void Start()
    {
        // SpawnEnemy();
        SpawnEnemyWave(3);
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, GenerateSpawnPosition(), Quaternion.identity);
    }

    public void SpawnEnemyWave(int enemysToSpawn)
    {
        for(int i = 0; i < enemysToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), Quaternion.identity);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-_spawnRange, _spawnRange);
        float spawnPosZ = Random.Range(-_spawnRange, _spawnRange);
        Vector3 spawnPos = new Vector3 (spawnPosX, 0, spawnPosZ);
        return spawnPos;
    }
}
