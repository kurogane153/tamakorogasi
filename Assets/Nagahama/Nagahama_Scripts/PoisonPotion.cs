using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPotion : MonoBehaviour
{
    // ボールの速度をへらす時間
    [SerializeField] private float _ballSpeedReduceTime = 5f;

    private BallControll ballControll;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball")) {
            ballControll = other.GetComponent<BallControll>();
            StartCoroutine(nameof(Poison));
        }
    }

    private IEnumerator Poison()
    {
        transform.position = new Vector3(10000, 10000, 10000);
        ballControll.isSpeedReduceHalf = true;
        yield return new WaitForSeconds(_ballSpeedReduceTime);
        ballControll.isSpeedReduceHalf = false;
    }
}
