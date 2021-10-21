using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorControll : MonoBehaviour
{
    [SerializeField] private float _horDeadZone = 0.3f;         // スティック入力水平方向デッドゾーン（この値以下の入力は入力してないことになる）
    [SerializeField] private float _verDeadZone = 0.3f;         // スティック入力垂直方向デッドゾーン
    [SerializeField] private float _reachTimeMaxAngle = 1.5f;   // 最大傾き角度に到達するまでの時間
    [SerializeField] private float _decelerateTimeStickNeutral = 1f;    // 傾けている途中にスティックが離されたとき傾く速度を0にするまでにかかる時間
    [SerializeField] private float _reachTimeNeutralAngle = 1.5f; // 元の角度にリセットされるまでの時間
    [SerializeField] private float _maxAngle = 30f;             // 最大傾き角度

    private float step = 0;
    private float returnStep = 0;
    private Vector3 lastAngle;
    private bool isStickReleased = false;

    private GUIStyle style;

    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 30;
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
        if      ( _verDeadZone < V && !isStickReleased) {
            step = Mathf.SmoothStep(step, 1, Time.deltaTime * _reachTimeMaxAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_maxAngle, 0, 0), step);
            lastAngle = new Vector3(_maxAngle, 0, 0);
            isStickReleased = false;

        }
        // 下入力
        else if ( V < -_verDeadZone && !isStickReleased) {
            step = Mathf.SmoothStep(step, 1, Time.deltaTime * _reachTimeMaxAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-_maxAngle, 0, 0), step);
            lastAngle = new Vector3(-_maxAngle, 0, 0);
            isStickReleased = false;

        }

        // 左入力
        if ( H < -_horDeadZone && !isStickReleased) {
            step = Mathf.SmoothStep(step, 1, Time.deltaTime * _reachTimeMaxAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, _maxAngle), step);
            lastAngle = new Vector3(0, 0, _maxAngle);
            isStickReleased = false;

        }
        // 右入力
        else if ( _horDeadZone < H && !isStickReleased) {
            step = Mathf.SmoothStep(step, 1, Time.deltaTime * _reachTimeMaxAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -_maxAngle), step);
            lastAngle = new Vector3(0, 0, -_maxAngle);
            isStickReleased = false;

        }

        // 入力なし
        if( _verDeadZone > V 
            && V > -_verDeadZone 
            && H > -_horDeadZone 
            && _horDeadZone > H ) {

            // 傾ける力がまだ残っていたら、それを0に戻す
            if(0 < step) {
                step = Mathf.SmoothStep(step, 0, Time.deltaTime * _decelerateTimeStickNeutral);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(lastAngle), step);
                isStickReleased = true;
            }

            // 傾ける力がなくなったら、水平に戻る
            if(step <= 0) {
                returnStep = Mathf.SmoothStep(returnStep, 1, Time.deltaTime * _reachTimeNeutralAngle);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), returnStep);
                isStickReleased = false;
            }
            
        }
    }

    private void OnGUI()
    {
        
        GUI.Label(new Rect(0, 180, 500, 100), "step: " + step, style);
        GUI.Label(new Rect(0, 230, 500, 100), "returnStep: " + returnStep, style);
        GUI.Label(new Rect(0, 280, 500, 100), "isStickReleased: " + isStickReleased, style);
    }
}
