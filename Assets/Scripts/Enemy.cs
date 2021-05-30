using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _speed = 3.0f;
    private Rigidbody _enemyRb;
    private GameObject _player;
    public bool isBoss = false;

    // ボスを実装するための変数群
    public float spawnInterval;         // ミニ敵を生み出す間隔
    private float nextSpawn;            // 次にミニ敵を生み出すタイミング
    public int miniEnemySpawnCount;     // 生み出すミニ敵の数
    private SpawnManager spawnManager;

    void Start()
    {
        _enemyRb = GetComponent<Rigidbody>();
        _player = GameObject.Find("Player");

        // enemy がボスの場合
        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (_player.transform.position - transform.position).normalized ;
        _enemyRb.AddForce(lookDirection * _speed);

        // ボスの時
        if(isBoss)
        {
            if(Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                // ボスが存在している限り、一定の時間間隔で MiniEnemyを召喚する
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }

        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
