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

    void Start()
    {
        _PlayerRb = GetComponent<Rigidbody>();
        _focalPoint = GameObject.Find("FocalPoint");
    }

    void Update()
    {
        MovePlayer();
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(powerupCountdownRoutine());
            powerupIndicator.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * powerupStrength , ForceMode.Impulse);
        }
    }

    // Coroutines To be able to create a new loop outside of our update loop
    IEnumerator powerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }
    void MovePlayer()
    {
        _forwardInput = Input.GetAxis("Vertical");
        _PlayerRb.AddForce(_focalPoint.transform.forward * speed * _forwardInput);
    }
}
