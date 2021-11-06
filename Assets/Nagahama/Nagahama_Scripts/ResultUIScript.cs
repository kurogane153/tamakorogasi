using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIScript : MonoBehaviour
{
    [SerializeField] private Text _playTimeText;  // クリア時間
    [SerializeField] private Text _coinCountText;   // コイン取得数
    [SerializeField] private GameObject _retryMenuPanel;    // リトライメニューパネルのオブジェクト

    private GameManager gm;

    void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnEnable()
    {
        Pauser.isCanNotPausing = true;

        if(gm == null) {
            gm = GameManager.Instance;
        }

        gm.OverCheck();

        int minutes = Mathf.FloorToInt(gm.PlayTime / 60F);
        int seconds = Mathf.FloorToInt(gm.PlayTime - minutes * 60);
        _playTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        _coinCountText.text = gm.CoinCount.ToString() + "枚";
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit")) {
            _retryMenuPanel.SetActive(true);
            gameObject.SetActive(false);
            SoundManager.Instance.PlaySystemSE(SystemSE.Decide);
        }
    }

}
