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

    public GameObject[] powerupPrefabs;

    void Start()
    {
        // PowerUp アイテムをランダムに生成
        SpawnPowerUpItem();
        // 敵を生成
        SpawnEnemyWave(waveNumber);
    }

    void Update()
    {
        // シーン内に存在している敵の数を取得し続ける
        enemyCount = FindObjectsOfType<Enemy>().Length;
        // 敵の数が 0 になった時、
        if(enemyCount == 0)
        {
            // wave の増加
            waveNumber++;
            // 敵の生成
            SpawnEnemyWave(waveNumber);
            // パワーアップアイテムの生成
            SpawnPowerUpItem();
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

    public void SpawnPowerUpItem()
    {
        // PowerUp アイテムをランダムに生成
        int randomPowerup = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);
    }

    // 生み出す位置
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-_spawnRange, _spawnRange);
        float spawnPosZ = Random.Range(-_spawnRange, _spawnRange);
        Vector3 spawnPos = new Vector3 (spawnPosX, 0, spawnPosZ);
        return spawnPos;
    }
}
