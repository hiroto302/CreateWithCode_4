using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _PlayerRb;
    public float speed = 5.0f;
    float _forwardInput;
    GameObject _focalPoint;
    // Start is called before the first frame update
    void Start()
    {
        _PlayerRb = GetComponent<Rigidbody>();
        _focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Debug.Log(_focalPoint.transform.forward);
    }
    void MovePlayer()
    {
        _forwardInput = Input.GetAxis("Vertical");
        _PlayerRb.AddForce(_focalPoint.transform.forward * speed * _forwardInput);
    }
}
