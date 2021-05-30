using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    public GameObject enemyPrefab;
    public GameObject strongEnemyPrefab;
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

    // wave が進ごとに、生み出す敵の数を増やしていく
    public void SpawnEnemyWave(int enemysToSpawn)
    {
        int enemysCount = enemysToSpawn;
        // waveが3進ごとに強キャラを一体生み出す
        if(enemysCount % 3 == 0)
        {
            enemysCount--;
            Instantiate(strongEnemyPrefab, GenerateSpawnPosition(), Quaternion.identity);
        }
        // 通常のenemy を生み出す処理
        for(int i = 0; i < enemysCount; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), Quaternion.identity);
        }
    }

    // 生まれる位置
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-_spawnRange, _spawnRange);
        float spawnPosZ = Random.Range(-_spawnRange, _spawnRange);
        Vector3 spawnPos = new Vector3 (spawnPosX, 0, spawnPosZ);
        return spawnPos;
    }
}
