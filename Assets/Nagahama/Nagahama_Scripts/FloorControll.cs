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
    [SerializeField] AnimationCurve _curve;

    private float step = 0;
    private float startTime;
    private Vector3 lastAngle;
    private bool isStickReleased = false;
    private bool isReturnHorizontal = false;
    private bool preStickDown = false;  // 前のフレームでスティックを倒していたか
    private Quaternion startRotation;

    private GUIStyle style;

    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 30;
        startTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // スティック入力を変数に入れてタイピングの手間を省く
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");

        if((_verDeadZone < V && !isStickReleased)||
            V < -_verDeadZone && !isStickReleased ||
            H < -_horDeadZone && !isStickReleased ||
            _horDeadZone < H && !isStickReleased) {

            if (!preStickDown) {
                startRotation = transform.rotation;
                preStickDown = true;
                startTime = Time.timeSinceLevelLoad;
            }

            

            step = Mathf.SmoothStep(step, 1, Time.deltaTime * _reachTimeMaxAngle);

            isStickReleased = false;
            isReturnHorizontal = false;

        }

        // 上入力
        if      ( _verDeadZone < V && !isStickReleased) {
            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;
            var pos = _curve.Evaluate(rate);
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(_maxAngle, 0, 0), pos);
            lastAngle = new Vector3(_maxAngle, 0, 0);


        }
        // 下入力
        else if ( V < -_verDeadZone && !isStickReleased) {
            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;
            var pos = _curve.Evaluate(rate);
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(-_maxAngle, 0, 0), pos);
            lastAngle = new Vector3(-_maxAngle, 0, 0);


        }

        // 左入力
        if ( H < -_horDeadZone && !isStickReleased) {
            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;
            var pos = _curve.Evaluate(rate);
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(0, 0, _maxAngle), pos);
            lastAngle = new Vector3(0, 0, _maxAngle);


        }
        // 右入力
        else if ( _horDeadZone < H && !isStickReleased) {
            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;
            var pos = _curve.Evaluate(rate);
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(0, 0, -_maxAngle), pos);
            lastAngle = new Vector3(0, 0, -_maxAngle);


        }

        // 入力なし
        if( _verDeadZone > V 
            && V > -_verDeadZone 
            && H > -_horDeadZone 
            && _horDeadZone > H ) {

            if (preStickDown) {
                startRotation = transform.rotation;
                preStickDown = false;
                startTime = Time.timeSinceLevelLoad;
            }

            // 傾ける力がまだ残っていたら、それを0に戻す
            if (0 < step) {
                var diff = Time.timeSinceLevelLoad - startTime;
                var rate = diff / _reachTimeNeutralAngle;
                var pos = _curve.Evaluate(rate);

                step = Mathf.SmoothStep(0, step, Time.deltaTime * _decelerateTimeStickNeutral);
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), pos);
                isStickReleased = true;

                if(step <= 0) {
                    isReturnHorizontal = true;
                    startTime = Time.timeSinceLevelLoad;
                    startRotation = transform.rotation;
                }
            }

            // 傾ける力がなくなったら、水平に戻る
            if(isReturnHorizontal) {
                var diff = Time.timeSinceLevelLoad - startTime;

                if (diff >= _reachTimeNeutralAngle) {
                    transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(0, 0, 0), 1);
                    Debug.Log(diff);
                    isReturnHorizontal = false;
                }

                var rate = diff / _reachTimeNeutralAngle;
                var pos = _curve.Evaluate(rate);
                
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(0, 0, 0), pos);
                isStickReleased = false;

                
            }
            
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 180, 500, 100), "step: " + step, style);
        GUI.Label(new Rect(0, 280, 500, 100), "isStickReleased: " + isStickReleased, style);



    }
}
