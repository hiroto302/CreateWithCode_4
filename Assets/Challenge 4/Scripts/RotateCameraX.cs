using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraX : MonoBehaviour
{
    private float speed = 200;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * speed * Time.deltaTime);

        // Rigidbody FreezeRotaiton x, y, z, are enable. So,The following process is required
        transform.position = player.transform.position; // Move focal point with player

    }
}
