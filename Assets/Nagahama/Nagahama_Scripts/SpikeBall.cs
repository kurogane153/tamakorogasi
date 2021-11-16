using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    // ボール衝突時、ボールを止めておく秒数
    [SerializeField] private float _ballStopTime = 1.5f;

    // ボール停止が解けたあと、トゲ球の当たり判定を停止しておく時間
    [SerializeField] private float _collisionSleepTime = 3f;

    private Rigidbody ballRB;   // 転がるボールのRigidBody
    private bool isCollisionSleep = false;  // トゲ鉄球のボールに対する当たり判定を行うか

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && !isCollisionSleep) {
            ballRB = collision.gameObject.GetComponent<Rigidbody>();
            
            StartCoroutine(nameof(TemporalBallStop));
        }
    }

    private IEnumerator TemporalBallStop()
    {
        float waitTime = 0;

        isCollisionSleep = true;

        while(waitTime < _ballStopTime) {
            waitTime += Time.deltaTime;
            ballRB.velocity = Vector3.zero;
            yield return new WaitForFixedUpdate();
        }

        // ボールを少し押し出す
        Vector3 vec = (ballRB.transform.position - transform.position).normalized;
        ballRB.AddForce((vec + Vector3.up) * 50f);

        yield return new WaitForSeconds(_collisionSleepTime);

        isCollisionSleep = false;
    }
}
