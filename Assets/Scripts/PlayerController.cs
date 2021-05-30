using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _PlayerRb;
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

    void Start()
    {
        _PlayerRb = GetComponent<Rigidbody>();
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
        _PlayerRb.AddForce(_focalPoint.transform.forward * speed * _forwardInput);
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
}
