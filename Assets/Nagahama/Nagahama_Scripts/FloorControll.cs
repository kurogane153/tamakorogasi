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

    private enum InputDirection
    {
        None,   // なし
        Up,     // 上
        Down,   // 下
        Left,   // 左
        Right   // 右
    }

    private InputDirection inDir = InputDirection.None;     // 今のフレームのスティック入力方向
    private InputDirection preInDir = InputDirection.None;  // 前のフレームのスティック入力方向

    private float preH;                     // 前フレームのスティック水平方向入力状態
    private float preV;                     // 前フレームのスティック垂直方向入力状態
    private float step = 0;                 // 床の傾く力を失わせる処理の分岐で参照する
    private float startTime;                // 床をn秒かけて倒すなどの処理の計測開始時間を入れる
    private float stickNeutralStartTime;    // 床の傾く力をn秒かけて倒す処理の計測開始時間を入れる
    private Vector3 lastAngle;              // 最後に傾く力をかけていた方向
    private bool isStickInput = true;      // スティックを入力しているか
    private bool isReturnHorizontal = false;    // 水平に戻ろうとしているか
    private bool preStickDown = false;  // 前のフレームでスティックを倒していたか
    private Quaternion startRotation;       // 床をn秒かけて倒すとき、倒し始めたフレームの床の角度

    private GUIStyle style;                 // デバッグ表示用

    private float dbgpos;                   // デバッグ表示用

    void Start()
    {
        // デバッグ用
        style = new GUIStyle();
        style.fontSize = 30;
    }

    private void FixedUpdate()
    {
        // スティック入力を変数に入れてタイピングの手間を省く
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");
        
        // スティックの軸が倒されていたら
        if((_verDeadZone < V && isStickInput) ||
            (V < -_verDeadZone && isStickInput) ||
            (H < -_horDeadZone && isStickInput) ||
            (_horDeadZone < H && isStickInput)) {

            // 前フレームでスティックを倒していなかったとき
            if (!preStickDown) {
                if(step <= 0) {
                    startRotation = transform.rotation;
                    startTime = Time.timeSinceLevelLoad;
                }
                
                preStickDown = true;
                
            }

            isStickInput = true;        
            isReturnHorizontal = false;

            // stepが0より上のときは床の傾く力を1秒かけて減速させる処理をする・
            if (0 < step) {
                // 1秒かけて減速、の計算に使う
                var diff2 = Time.timeSinceLevelLoad - stickNeutralStartTime;
                var rate2 = diff2 / _decelerateTimeStickNeutral;

                // もともと床にかかっていた力
                var diff = Time.timeSinceLevelLoad - startTime;
                var rate = diff / (_reachTimeNeutralAngle + (_reachTimeNeutralAngle / 2) * rate2);

                // アニメーションカーブを使って、徐々に加速し、トップスピードなる、という表現をさせる
                var pos = _increaseCurve.Evaluate(rate);
                dbgpos = pos;

                // 補完を使って傾きの計算をする
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), pos);

                // 床をn秒かけて減速という処理、その秒数に達したら、もとの力をかける処理に戻る
                if (diff2 >= _decelerateTimeStickNeutral) {
                    startRotation = transform.rotation;
                    startTime = Time.timeSinceLevelLoad;
                    preStickDown = false;
                    step = 0;
                }

                preInDir = inDir;
                preH = H;
                preV = V;
                return; // 早期リターン
            }

        }

        // 上入力
        if      ( _verDeadZone < V && isStickInput) {

            inDir = InputDirection.Up;

            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;
            var pos = _increaseCurve.Evaluate(rate);
            dbgpos = pos;

            if (preInDir != InputDirection.Up && preInDir != InputDirection.None) {
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;
                return;
            }
            
            lastAngle.x = _maxAngle;

            if( H < -_horDeadZone && isStickInput && !(preH < -_horDeadZone)) {
                // 追加左入力があったら、左奥に傾けられるようにする
                //lastAngle.z = _maxAngle;
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;

            } else if ( _horDeadZone < H && isStickInput && !(_horDeadZone < -preH)) {
                // 追加右入力があったら、右奥に傾けられるようにする
                //lastAngle.z = -_maxAngle;
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;
            }

            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), pos);

        }
        // 下入力
        else if ( V < -_verDeadZone && isStickInput) {
            inDir = InputDirection.Down;

            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;
            var pos = _increaseCurve.Evaluate(rate);
            dbgpos = pos;

            if (preInDir != InputDirection.Down && preInDir != InputDirection.None) {
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;
                return;
            }
           
            lastAngle.x = -_maxAngle;

            if (H < -_horDeadZone && isStickInput && !(preH < -_horDeadZone)) {
                // 追加左入力があったら、左手前に傾けられるようにする
                //lastAngle.z = _maxAngle;
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;

            } else if (_horDeadZone < H && isStickInput && !(_horDeadZone < -preH)) {
                // 追加右入力があったら、右手前に傾けられるようにする
                //lastAngle.z = -_maxAngle;
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;
            }

            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), pos);

        }

        // 左入力
        else if ( H < -_horDeadZone && isStickInput) {

            inDir = InputDirection.Left;

            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;
            var pos = _increaseCurve.Evaluate(rate);
            dbgpos = pos;

            if (preInDir != InputDirection.Left && preInDir != InputDirection.None) {
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;
                return;
            }
            
            lastAngle.z = _maxAngle;

            if ( _verDeadZone < V && isStickInput && !(_verDeadZone < preV)) {
                // 追加上入力があったら、左奥に傾けられるようにする
                //lastAngle.x = _maxAngle;
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;

            } else if ( V < -_verDeadZone && isStickInput && !(preV < -_verDeadZone)) {
                // 追加下入力があったら、左手前に傾けられるようにする
                //lastAngle.x = -_maxAngle;
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;
            }

            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), pos);

        }
        // 右入力
        else if ( _horDeadZone < H && isStickInput) {
            inDir = InputDirection.Right;

            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / _reachTimeNeutralAngle;
            var pos = _increaseCurve.Evaluate(rate);
            dbgpos = pos;

            if (preInDir != InputDirection.Right && preInDir != InputDirection.None) {
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;
                return;
            }
            
            lastAngle.z = -_maxAngle;

            if (_verDeadZone < V && isStickInput && !(_verDeadZone < preV)) {
                // 追加上入力があったら、右奥に傾けられるようにする
                //lastAngle.x = _maxAngle;
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;

            } else if (V < -_verDeadZone && isStickInput && !(preV < -_verDeadZone)) {
                // 追加下入力があったら、右手前に傾けられるようにする
                //lastAngle.x = -_maxAngle;
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                step = 1;
                preH = H;
                preV = V;
            }

            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), pos);

        }

        // 入力なし
        if( _verDeadZone > V 
            && V > -_verDeadZone 
            && H > -_horDeadZone 
            && _horDeadZone > H || 0 < step) {

            inDir = InputDirection.None;

            if (preStickDown) {
                stickNeutralStartTime = Time.timeSinceLevelLoad;
                preStickDown = false;
                step = 1;
            }

            // 傾ける力がまだ残っていたら、それを0に戻す
            if (!isReturnHorizontal && 0 < step) {
                var diff2 = Time.timeSinceLevelLoad - stickNeutralStartTime;
                var rate2 = diff2 / _decelerateTimeStickNeutral;

                var diff = Time.timeSinceLevelLoad - startTime;
                var rate = diff / (_reachTimeNeutralAngle + (_reachTimeNeutralAngle / 2) * rate2);
                
                var pos = _increaseCurve.Evaluate(rate);
                dbgpos = pos;

                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(lastAngle), pos);
                isStickInput = false;

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
                isStickInput = true;
                
            }
            
        }

        preInDir = inDir;
        preH = H;
        preV = V;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 180, 500, 300), "入力方向 : " + inDir, style);
    }
}
