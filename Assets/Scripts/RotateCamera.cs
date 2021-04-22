using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 50.0f;
    float _horizontalInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateViewPoint();
    }

    void RotateViewPoint()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * _horizontalInput);
    }
}
