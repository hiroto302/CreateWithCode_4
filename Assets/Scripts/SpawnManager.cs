using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    public GameObject enemyPrefab;
    private float _spawnRange = 9;
    public int enemyCount;

    public int waveNumber = 1;

    public GameObject powerupPrefab;

    void Start()
    {
        SpawnObject(powerupPrefab);
        SpawnEnemyWave(1);
    
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if(enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            SpawnObject(powerupPrefab);
        }
    }

    public void SpawnObject(GameObject obj)
    {
        Instantiate(obj, GenerateSpawnPosition(), Quaternion.identity);
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
