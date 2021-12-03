using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceCalc : MonoBehaviour
{
    //ボールが当たった物体の法線ベクトル
    private Vector3 objNomalVector = Vector3.zero;
    // ボールのrigidbody
    private Rigidbody rb;

    private BallControll ballcontroll;
    // 跳ね返った後のverocity
    [HideInInspector] public Vector3 afterReflectVero = Vector3.zero;

    // 通常の反射力
    [SerializeField] private float _bouncePower = 1f;

    // アイテムで強化されている間適用される反射力
    [SerializeField] private float _itemEnhancedBouncePower = 1.3f;

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            if (ballcontroll.isBounceUp)
            {
                // 当たった物体の法線ベクトルを取得+
                objNomalVector = collision.contacts[0].normal;
                Vector3 reflectVec2 = Vector3.Reflect(afterReflectVero, objNomalVector * _itemEnhancedBouncePower);
                rb.velocity = reflectVec2;

                // 計算した反射ベクトルを保存
                afterReflectVero = rb.velocity;
                afterReflectVero = afterReflectVero / _itemEnhancedBouncePower;
            }
            else
            {
                // 当たった物体の法線ベクトルを取得+
                objNomalVector = collision.contacts[0].normal;
                Vector3 reflectVec = Vector3.Reflect(afterReflectVero, objNomalVector * _bouncePower);
                rb.velocity = reflectVec;
                // 計算した反射ベクトルを保存
                afterReflectVero = rb.velocity;
                //afterReflectVero = afterReflectVero / 1.2f;
                //Debug.Log("nomal:" + afterReflectVero);
            }

        }
        
    }

    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        ballcontroll = this.GetComponent<BallControll>();
    }
}
