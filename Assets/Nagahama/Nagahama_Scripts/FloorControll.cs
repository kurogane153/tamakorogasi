using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorControll : MonoBehaviour
{
    [SerializeField] private float _horDeadZone = 0.3f;         // スティック入力水平方向デッドゾーン（この値以下の入力は入力してないことになる）
    [SerializeField] private float _verDeadZone = 0.3f;         // スティック入力垂直方向デッドゾーン
    [SerializeField] private float _reachTimeMaxAngle = 1.5f;   // 最大傾き角度に到達するまでの時間
    [SerializeField] private float _reachTimeNeutralAngle = 1f; // 元の角度にリセットされるまでの時間
    [SerializeField] private float _maxAngle = 30f;             // 最大傾き角度

    private float step = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // スティック入力を変数に入れてタイピングの手間を省く
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");

        

        // 上入力
        if      ( _verDeadZone < V ) {
            step = Mathf.Lerp(step, _reachTimeMaxAngle, Time.deltaTime * _reachTimeMaxAngle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_maxAngle, 0, 0), step);

        }
        // 下入力
        else if ( V < -_verDeadZone ) {
            step = Mathf.Lerp(step, _reachTimeMaxAngle, Time.deltaTime * _reachTimeMaxAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(-_maxAngle, 0, 0), step);

        }

        // 左入力
        if ( H < -_horDeadZone ) {
            step = Mathf.Lerp(step, _reachTimeMaxAngle, Time.deltaTime * _reachTimeMaxAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, _maxAngle), step);

        }
        // 右入力
        else if ( _horDeadZone < H ) {
            step = Mathf.Lerp(step, _reachTimeMaxAngle, Time.deltaTime * _reachTimeMaxAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -_maxAngle), step);

        }

        // 入力なし
        if( _verDeadZone > V 
            && V > -_verDeadZone 
            && H > -_horDeadZone 
            && _horDeadZone > H ) {

            step = Mathf.Lerp(step, _reachTimeNeutralAngle, Time.deltaTime * _reachTimeNeutralAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), step);
        }
    }
}
