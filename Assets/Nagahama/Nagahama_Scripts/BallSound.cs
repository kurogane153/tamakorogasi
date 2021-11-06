using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSound : MonoBehaviour
{
    // この値以上の速度が出ていたら転がっているゴロゴロ音を鳴らす
    [SerializeField] private float _rollingSoundPlaySpeed = 0.1f;
    [SerializeField] private float _rollingSoundStopWaitTime = 1f;
    private float waitTime;

    private Vector3 preTargetPos;                   // 前フレームでの位置
    private float continuousMoveDistance;           // 連続して移動した距離
    private Vector3 lastCountinuousMovePos;         // 連続していたときの最終位置

    private float _fadeoutSeconds = 1f;
    private AudioSource audioSource;
    private bool isFadeout = false;
    private bool isFadein = false;
    private float fadeDeltaTime = 0;
    private float startVolume;

    private GUIStyle style;                 // デバッグ表示用
    private bool debug = false;

    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 30;
        audioSource = GetComponent<AudioSource>();
        startVolume = 1;
        audioSource.volume = 0;
    }

    private void FixedUpdate()
    {
        // ターゲットの今のフレームでの位置と前フレームでの位置との距離がinspectorで設定した値以上なら、「移動した」とみなす
        Vector3 prePos = preTargetPos;
        prePos.y = 0;

        Vector3 newPos = transform.position;
        newPos.y = 0;

        float targetPrePosDistance = Vector3.Distance(newPos, prePos);
        

        if (_rollingSoundPlaySpeed < targetPrePosDistance) {
            continuousMoveDistance += targetPrePosDistance;
            lastCountinuousMovePos = transform.position;
            if (!isFadein) FadeinStart(_fadeoutSeconds);
            debug = true;
            waitTime = 0;
        } else {
            waitTime += Time.deltaTime;
            if(_rollingSoundStopWaitTime <= waitTime) {
                if (!isFadeout) FadeoutStart(_fadeoutSeconds);
            }
            
            debug = false;
        }

        preTargetPos = transform.position;
    }

    private void FadeoutStart(float interval)
    {
        StopCoroutine(nameof(Fadein));
        isFadein = false;
        isFadeout = true;
        startVolume = audioSource.volume;
        //audioSource.volume = startVolume;
        _fadeoutSeconds = interval - 0.017f;
        fadeDeltaTime = 0;
        StartCoroutine(nameof(Fadeout));
    }

    private void FadeinStart(float interval)
    {
        StopCoroutine(nameof(Fadeout));
        isFadeout = false;
        isFadein = true;
        startVolume = audioSource.volume;
        //audioSource.volume = startVolume;
        _fadeoutSeconds = interval - 0.017f;
        fadeDeltaTime = 0;
        StartCoroutine(nameof(Fadein));
    }

    private IEnumerator Fadeout()
    {
        while (fadeDeltaTime < _fadeoutSeconds) {
            fadeDeltaTime += Time.deltaTime;
            audioSource.volume -= _fadeoutSeconds * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        audioSource.volume = 0;
        fadeDeltaTime = _fadeoutSeconds;
        //isFadeout = false;
    }

    private IEnumerator Fadein()
    {
        while (fadeDeltaTime < _fadeoutSeconds) {
            fadeDeltaTime += Time.deltaTime;
            audioSource.volume += _fadeoutSeconds * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        audioSource.volume = 1;
        fadeDeltaTime = _fadeoutSeconds;
        //isFadein = false;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 180, 500, 300), "DEBUG : " + debug, style);
    }
}
