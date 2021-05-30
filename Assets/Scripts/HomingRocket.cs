using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingRocket : MonoBehaviour
{
    private Transform target;
    private float speed = 15.0f;
    private bool homing;
    private float rocketStrength = 15.0f;
    private float aliveTimer = 5.0f;

    void Update()
    {
        if(homing && target != null)
        {
            // 弾丸制御
            // 飛ぶ方向を取得
            Vector3 moveDirection = (target.transform.position -transform.position).normalized;
            // その方向に移動していく
            transform.position += moveDirection * speed * Time.deltaTime;
            // targetの方向に方向転換する
            transform.LookAt(target);
        }
    }

    // ロケットがTargetに衝突した時の処理
    void OnCollisionEnter(Collision col)
    {
        if (target != null)
        {
            if (col.gameObject.CompareTag(target.tag))
            {
                // 衝突する相手のRigidbodyを取得
                Rigidbody targetRigidbody = col.gameObject.GetComponent<Rigidbody>();
                // We then use the normal of the collision contact to determine which direction to push the target in.
                Vector3 away = -col.contacts[0].normal;
                targetRigidbody.AddForce(away * rocketStrength, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }

    public void Fire(Transform newTarget)
    {
        target = newTarget;
        homing = true;
        // 設定した秒数後に削除
        Destroy(gameObject, aliveTimer);
    }
}
