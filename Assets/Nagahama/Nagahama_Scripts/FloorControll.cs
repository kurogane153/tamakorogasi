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
    [SerializeField] AnimationCurve _increaseCurve;         // 加速に使うカーブ
    [SerializeField] AnimationCurve _decreaseCurve; // 減速に使うカーブ
    

    private enum InputDirection
    {
        None,   // なし
        Up,     // 上
        Down,   // 下
        Left,   // 左
        Right   // 右
    }

    private InputDirection inDir = InputDirection.None;
    private float step = 0;
    private float startTime;
    private float startTime2;
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
        startTime2 = Time.timeSinceLevelLoad;
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
            (V < -_verDeadZone && !isStickReleased) ||
            (H < -_horDeadZone && !isStickReleased) ||
            (_horDeadZone < H && !isStickReleased)) {

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

            if (inDir != InputDirection.Up && inDir != InputDirection.None) {
                rate = diff / _decelerateTimeStickNeutral;
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), rate);

                if (diff >= _reachTimeNeutralAngle) {
                    startRotation = transform.rotation;
                    startTime = Time.timeSinceLevelLoad;
                    preStickDown = false;
                }
                return;
            }


            inDir = InputDirection.Up;

            
            var pos = _increaseCurve.Evaluate(rate);
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(_maxAngle, 0, 0), pos);
            lastAngle = new Vector3(_maxAngle, 0, 0);


        }
        // 下入力
        else if ( V < -_verDeadZone && !isStickReleased) {
            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;

            if (inDir != InputDirection.Down && inDir != InputDirection.None) {
                rate = diff / _decelerateTimeStickNeutral;
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), rate);

                if (diff >= _reachTimeNeutralAngle) {
                    startRotation = transform.rotation;
                    startTime = Time.timeSinceLevelLoad;
                    preStickDown = false;
                }
                return;
            }


            inDir = InputDirection.Down;
            var pos = _increaseCurve.Evaluate(rate);
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(-_maxAngle, 0, 0), pos);
            lastAngle = new Vector3(-_maxAngle, 0, 0);


        }

        // 左入力
        else if ( H < -_horDeadZone && !isStickReleased) {
            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;

            if (inDir != InputDirection.Left && inDir != InputDirection.None) {
                rate = diff / _decelerateTimeStickNeutral;
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), rate);

                if (diff >= _reachTimeNeutralAngle) {
                    startRotation = transform.rotation;
                    startTime = Time.timeSinceLevelLoad;
                    preStickDown = false;
                }
                return;
            }


            inDir = InputDirection.Left;
            var pos = _increaseCurve.Evaluate(rate);
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(0, 0, _maxAngle), pos);
            lastAngle = new Vector3(0, 0, _maxAngle);


        }
        // 右入力
        else if ( _horDeadZone < H && !isStickReleased) {
            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;

            if (inDir != InputDirection.Right && inDir != InputDirection.None) {
                rate = diff / _decelerateTimeStickNeutral;
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), rate);

                if (diff >= _reachTimeNeutralAngle) {
                    startRotation = transform.rotation;
                    startTime = Time.timeSinceLevelLoad;
                    preStickDown = false;
                }
                return;
            }


            inDir = InputDirection.Right;
            var pos = _increaseCurve.Evaluate(rate);
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(0, 0, -_maxAngle), pos);
            lastAngle = new Vector3(0, 0, -_maxAngle);


        }

        // 入力なし
        if( _verDeadZone > V 
            && V > -_verDeadZone 
            && H > -_horDeadZone 
            && _horDeadZone > H ) {

            inDir = InputDirection.None;

            if (preStickDown) {
                //startRotation = transform.rotation;
                startTime2 = Time.timeSinceLevelLoad;
                preStickDown = false;
                //startTime = Time.timeSinceLevelLoad;
            }

            // 傾ける力がまだ残っていたら、それを0に戻す
            if (!isReturnHorizontal && 0 < step) {
                var diff2 = Time.timeSinceLevelLoad - startTime2;
                var rate2 = diff2 / _decelerateTimeStickNeutral;

                var diff = Time.timeSinceLevelLoad - startTime;
                var rate = diff / (_reachTimeNeutralAngle + (_reachTimeNeutralAngle / 2) * rate2);
                
                var pos = _increaseCurve.Evaluate(rate);

                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), pos);
                isStickReleased = true;

                if(diff2 >= _decelerateTimeStickNeutral) {
                    isReturnHorizontal = true;
                    startTime = Time.timeSinceLevelLoad;
                    startRotation = transform.rotation;
                    step = 0;
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
                var pos = _increaseCurve.Evaluate(rate);
                
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(0, 0, 0), pos);
                isStickReleased = false;

                
            }
            
        }
    }

    private void OnGUI()
    {
        if (!isReturnHorizontal && 0 < step) {
            var diff2 = Time.timeSinceLevelLoad - startTime2;
            var rate2 = diff2 / _decelerateTimeStickNeutral;

            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / (_reachTimeNeutralAngle + (_reachTimeNeutralAngle / 2) * rate2);
            GUI.Label(new Rect(0, 180, 500, 100), "diff: " + diff, style);
            GUI.Label(new Rect(0, 230, 500, 100), "rate: " + rate, style);
            GUI.Label(new Rect(0, 280, 500, 100), "diff2: " + diff2, style);
            GUI.Label(new Rect(0, 330, 500, 100), "rate2: " + rate2, style);
        }



    }
}
