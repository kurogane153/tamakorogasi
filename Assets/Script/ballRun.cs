using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballRun : MonoBehaviour
{
    private Rigidbody rb;
    private float speed;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.angularVelocity = new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.angularVelocity = new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.angularVelocity = new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.angularVelocity = new Vector3(0, 0, 1);
        }
    }
}
