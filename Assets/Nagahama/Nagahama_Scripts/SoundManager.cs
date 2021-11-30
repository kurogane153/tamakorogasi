﻿using UnityEngine;
using System;
using System.Collections;

public enum SE
{
    CountDown,
    Start,
    Rolling,
    Conflict,
    Coin,
    Explosion,
    Fuse,

    MAX
}

public enum SystemSE
{
    Cursor,
    Decide,

    MAX
}


// 音管理クラス
public class SoundManager : MonoBehaviour
{

    #region シングルトン
    protected static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null) {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));

                if (instance == null) {
                    Debug.LogError("SoundManager Instance Error");
                }
            }

            return instance;
        }
    }

    #endregion

    #region 変数
    // 音量
    public SoundVolume volume = new SoundVolume();

    // === AudioSource ===
    // BGM
    private AudioSource BGMsource;
    // SE
    private AudioSource[] SEsources = new AudioSource[32];
    // 音声
    private AudioSource[] SystemSEsources = new AudioSource[16];

    // === AudioClip ===
    // BGM
    public AudioClip[] BGM;
    // SE
    public AudioClip[] SE;
    // 音声
    public AudioClip[] SystemSE;

    #endregion

    void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("SoundManager");
        if (obj.Length > 1) {
            // 既に存在しているなら削除
            Destroy(gameObject);
        } else {
            // 音管理はシーン遷移では破棄させない
            DontDestroyOnLoad(gameObject);
        }

        // 全てのAudioSourceコンポーネントを追加する

        // BGM AudioSource
        BGMsource = gameObject.AddComponent<AudioSource>();
        // BGMはループを有効にする
        BGMsource.loop = true;

        // SE AudioSource
        for (int i = 0; i < SEsources.Length; i++) {
            SEsources[i] = gameObject.AddComponent<AudioSource>();
        }

        // 音声 AudioSource
        for (int i = 0; i < SystemSEsources.Length; i++) {
            SystemSEsources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // ミュート設定
        BGMsource.mute = volume.Mute;
        foreach (AudioSource source in SEsources) {
            source.mute = volume.Mute;
        }
        foreach (AudioSource source in SystemSEsources) {
            source.mute = volume.Mute;
        }

        // ボリューム設定
        BGMsource.volume = volume.BGM;
        foreach (AudioSource source in SEsources) {
            source.volume = volume.SE;
        }
        foreach (AudioSource source in SystemSEsources) {
            source.volume = volume.SystemSE;
        }

        
    }

    // ***** BGM再生 *****
    // BGM再生
    public void PlayBGM(int index)
    {
        if (0 > index || BGM.Length <= index) {
            return;
        }
        // 同じBGMの場合は何もしない
        if (BGMsource.clip == BGM[index]) {
            return;
        }
        BGMsource.Stop();
        BGMsource.clip = BGM[index];
        BGMsource.Play();
    }

    // BGM一時停止
    public void PauseBGM()
    {
        BGMsource.Pause();
    }

    // BGM再生再開
    public void UnPauseBGM()
    {
        BGMsource.UnPause();
    }

    // BGM停止
    public void StopBGM()
    {
        BGMsource.Stop();
        BGMsource.clip = null;
    }


    // ***** SE再生 *****
    // SE再生
    public void PlaySE(SE index)
    {
        if (0 > index || SE.Length <= ((int)index)) {
            return;
        }

        // 再生中で無いAudioSouceで鳴らす
        foreach (AudioSource source in SEsources) {
            if (false == source.isPlaying) {
                source.clip = SE[((int)index)];
                source.Play();
                return;
            }
        }
    }

    // SE一時停止
    public void PauseSE()
    {
        foreach (AudioSource source in SEsources) {
            source.Pause();
        }
    }

    // SE再生再開
    public void UnPauseSE()
    {
        foreach (AudioSource source in SEsources) {
            source.UnPause();
        }
    }

    // SE停止
    public void StopSE()
    {
        // 全てのSE用のAudioSouceを停止する
        foreach (AudioSource source in SEsources) {
            source.Stop();
            source.clip = null;
        }
    }


    // ***** 音声再生 *****
    // 音声再生
    public void PlaySystemSE(SystemSE index)
    {
        if (0 > index || SystemSE.Length <= ((int)index)) {
            return;
        }
        // 再生中で無いAudioSouceで鳴らす
        foreach (AudioSource source in SystemSEsources) {
            if (false == source.isPlaying) {
                source.clip = SystemSE[((int)index)];
                source.Play();
                return;
            }
        }
    }

    // 音声停止
    public void StopSystemSE()
    {
        // 全ての音声用のAudioSouceを停止する
        foreach (AudioSource source in SystemSEsources) {
            source.Stop();
            source.clip = null;
        }
    }
}

// 音量クラス
[Serializable]
public class SoundVolume
{
    public float BGM = 1.0f;
    public float SystemSE = 1.0f;
    public float SE = 1.0f;
    public bool Mute = false;

    public void Init()
    {
        BGM = 1.0f;
        SystemSE = 1.0f;
        SE = 1.0f;
        Mute = false;
    }
}