using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallControll : MonoBehaviour
{
    private Vector3 startPos;
    private Rigidbody rb;

    private int itemCount;

    public GameObject GameClearText;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        itemCount = 0;
        if (rb) {
            Debug.Log("ボールにRigidbodyがアタッチされている");
        }
        
    }

    void Update()
    {
        //if (Input.GetButtonDown("Cancel")) {
            //transform.position = startPos;
            //if (rb) {
                //rb.velocity = Vector3.zero;
            //}
        //}
    }

    void FixedUpdate()
    {
        if (itemCount == 12)
        {
            GameClearText.SetActive(true);
            Time.timeScale = 0; 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            Debug.Log("すり抜けた！");
            itemCount++;
            other.gameObject.SetActive(false);
            Debug.Log(itemCount);
        }
        
    }
    public int GetItemCount()
    {
        return itemCount;
    }

}
