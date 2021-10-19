using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField] private Transform _target;         // 追尾する対象

    [SerializeField, Tooltip("オフにすると、シーンビュー上での現在のカメラとボールの距離が相対距離となります")] 
        private bool _isUseOffsetProperty = false;      // 下のoffsetプロパティを使用するか

    [SerializeField] private Vector3 _offset;           // オフセット

    [SerializeField, Tooltip("この距離以上連続して移動すると追尾をする")]
    private float _followContinuousMoveDistance = 0.15f;

    [SerializeField, Tooltip("前フレームの位置からの距離がこの値以上なら、「連続して移動した」とする")]
    private float _followPrePosDifferenceDisctace = 0.05f;

    private Vector3 preTargetPos;                   // 前フレームでの位置
    private float continuousMoveDistance;           // 連続して移動した距離
    private Vector3 lastCountinuousMovePos;         // 連続していたときの最終位置

    void Start()
    {
        //自分自身とtargetとの相対距離を求める
        if (!_isUseOffsetProperty) {
            _offset = transform.position - _target.position;
        }
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = _target.position + _offset;
        targetPos.y = transform.position.y;

        // ターゲットの今のフレームでの位置と前フレームでの位置との距離がinspectorで設定した値以上なら、「移動した」とみなす
        float targetPrePosDistance = Vector3.Distance(_target.position, preTargetPos);
        if (_followPrePosDifferenceDisctace <= targetPrePosDistance) {
            continuousMoveDistance += targetPrePosDistance;
            lastCountinuousMovePos = _target.position;
            Debug.Log("連続移動");
        } else {
            // 設定した値以下であれば止まったとみなし、合計移動距離をリセットする
            continuousMoveDistance = 0;
        }

        if (_followContinuousMoveDistance <= continuousMoveDistance) {
            // 自分自身の座標に、targetの座標に相対座標を足した値を設定する
            transform.position = Vector3.Lerp(transform.position, targetPos, 6.0f * Time.deltaTime);
        }

        if(_followContinuousMoveDistance <= Vector3.Distance(_target.position, lastCountinuousMovePos)) {
            // 自分自身の座標に、targetの座標に相対座標を足した値を設定する
            transform.position = Vector3.Lerp(transform.position, targetPos, 6.0f * Time.deltaTime);
            //lastCountinuousMovePos = _target.position;
            Debug.Log("最低移動");
        }

        preTargetPos = _target.position;

    }
}
