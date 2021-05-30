using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _playerRb;
    public float speed = 5.0f;
    float _forwardInput;
    GameObject _focalPoint;
    // has : ~した
    public bool hasPowerup;
    float powerupStrength = 15.0f;
    public GameObject powerupIndicator;
    public PowerUpType currentPowerUp = PowerUpType.None;
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;

    // Smash機能関連の変数群
    public float hangTime = 0.5f ;          // 上昇させるのかける時間
    public float smashSpeed = 5.0f;        // 上に飛ぶスピード
    public float explosionForce = 50.0f;    // 敵に与える威力
    public float explosionRadius = 10.0f;   // 範囲??
    bool smashing = false;          // スマッシュ攻撃の最中であるか
    float floorY;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _focalPoint = GameObject.Find("FocalPoint");
    }

    void Update()
    {
        MovePlayer();
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        // ロケットを飛ばす処理
        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }
        // スマッシュ攻撃
        if(currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            StartCoroutine(Smash());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // PowerUp アイテムに触れた時
        if(other.CompareTag("Powerup"))
        {
            // powerUp の状態になる
            hasPowerup = true;
            // 接触したpowerUpアイテムはどんなタイプか
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            Destroy(other.gameObject);
            if(powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());

            // パワーアップしてる状態を表す,インディケータを表示
            powerupIndicator.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // パワーアップしてる状態の時、敵と触れた時の処理
        if(collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * powerupStrength , ForceMode.Impulse);
            Debug.Log("Player collided with: " + collision.gameObject.name + " withpowerup set to " + currentPowerUp.ToString());
        }
    }

    // Coroutines To be able to create a new loop outside of our update loop
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        // 7秒後に,パワーアップする前の状態に戻す
        hasPowerup = false;
        powerupIndicator.SetActive(false);
        currentPowerUp = PowerUpType.None;
    }
    void MovePlayer()
    {
        _forwardInput = Input.GetAxis("Vertical");
        _playerRb.AddForce(_focalPoint.transform.forward * speed * _forwardInput);
    }

    // ホーミングロケットを発射する処理
    void LaunchRockets()
    {
        // 全ての敵を取得して処理をしていく
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            // ロケットを生成し、敵に向けて飛ばしていく
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up,
            Quaternion.identity);
            tmpRocket.GetComponent<HomingRocket>().Fire(enemy.transform);
        }
    }

    // スマッシュ攻撃を行うための移動処理
    IEnumerator Smash()
    {
        // シーン内に存在する全ての敵を取得
        var enemies = FindObjectsOfType<Enemy>();
        //Store the y position before taking off : 上に飛ぶ前の位置
        floorY = transform.position.y;
        //Calculate the amount of time we will go up : 上がるためにかける時間の量を計算
        float jumpTime = Time.time + hangTime;
        // 上に上昇させる処理
        while(Time.time < jumpTime)
        {
            //move the player up while still keeping their x velocity : X方向の速度を維持したまま、プレイヤーを上方向に移動させる
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, smashSpeed);
            yield return null;
        }
        //Now move the player down : 下に下降させる処理
        while(transform.position.y > floorY)
        {
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }
        //Cycle through all enemies.
        for (int i = 0; i < enemies.Length; i++)
        {
            //Apply an explosion force that originates from our position : 自分の位置を起点とした爆発力を敵に加える
            if(enemies[i] != null)
            enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
            transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
        }
        //We are no longer smashing, so set the boolean to false : スマッシュ攻撃の終了
        smashing = false;
    }
}
