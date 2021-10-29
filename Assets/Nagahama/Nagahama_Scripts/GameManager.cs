using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null) {
                instance = (GameManager)FindObjectOfType(typeof(GameManager));

                if (instance == null) {
                    Debug.LogError(typeof(GameManager) + "is nothing");
                }
            }

            return instance;
        }
    }

    #endregion Singleton

    private void Awake()
    {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // ステージごとのプレイ時間
    private float playTime;

    public float PlayTime
    {
        get { return playTime; }
        set { playTime = value; }
    }

    // コイン取得数
    private int coinCount;

    public int CoinCount
    {
        get { return coinCount; }
        set { coinCount = value; }
    }

    public void StageScoreReset()
    {
        playTime = 0f;
        coinCount = 0;
        
    }

    public void OverCheck()
    {
        if (359940f < playTime) playTime = 359940f;

        if (999 < coinCount) coinCount = 999;
        
    }

}