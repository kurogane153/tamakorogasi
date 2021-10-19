using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControll : MonoBehaviour
{
    private Vector3 startPos;
    private Rigidbody rb;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        if (rb) {
            Debug.Log("ボールにRigidbodyがアタッチされている");
        }
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            transform.position = startPos;
            if (rb) {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
