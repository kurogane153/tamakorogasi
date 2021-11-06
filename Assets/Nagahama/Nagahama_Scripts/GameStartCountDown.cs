using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartCountDown : MonoBehaviour
{
    [SerializeField] private int _countDownTime = 3;    // 何秒カウントダウンするか
    private Text countDownText;             // カウントダウンの文字列を入れるオブジェクトのTextコンポーネント
    private FloorControll floorControll;    // シーンにおいてあるFloorControll
    private Image image;  // 自分のImage
    private SoundManager sm;

    void Start()
    {
        image = GetComponent<Image>();    // 自分のImage 取得
        countDownText = GetComponentInChildren<Text>();     // 子要素のText 取得
        countDownText.text = _countDownTime.ToString();
        sm = SoundManager.Instance;         // SoundManager のインスタンスを取得しておき、コーディングしやすくする
        floorControll = FindObjectOfType<FloorControll>();  // シーンからFloorControll 取得
        floorControll.enabled = false;                      // FloorControll 非アクティブにする

        // 非表示にしておく
        image.enabled = false;
        countDownText.enabled = false;

        // カウントダウン開始
        StartCoroutine(nameof(CountDown));
    }

    /// <summary>
    /// カウントダウン演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator CountDown()
    {
        // 最初に1秒待つ
        yield return new WaitForSeconds(1f);

        // 表示
        image.enabled = true;
        countDownText.enabled = true;

        // カウントダウン
        while (0 < _countDownTime) {
            countDownText.text = _countDownTime.ToString();
            _countDownTime--;
            sm.PlaySE(SE.CountDown);
            yield return new WaitForSeconds(1f);
        }

        // スタート！表示
        countDownText.text = "スタート！";
        sm.PlaySE(SE.Start);
        yield return new WaitForSeconds(0.5f);

        // BGM再生開始
        sm.PlayBGM(0);

        // FloorControll オンにする
        floorControll.enabled = true;

        // 自分を非アクティブにする
        gameObject.SetActive(false);

    }
}
