using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathSe : MonoBehaviour
{
    [System.Serializable]
    enum State
    {
        Normal,     // 通常
        Middle,     // 息切れ
        High,       // 大きな息切れ
        VeryHigh    // とても大きな息切れ
    };
    [SerializeField]
    AudioSource source = default;

    [SerializeField]
    State currentState = State.Normal;

    [SerializeField] AudioClip   holdClip = default;
    [SerializeField] float       holdPitch = default;
    [SerializeField] float[]     holdVolume = { 0.2f, 0.5f, 0.5f, 0.5f };
    [SerializeField] AudioClip[] breathEndClip = default;
    [SerializeField] float[]     pitchEnd = { 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] float[]     volumeEnd = { 0, 0.1f, 0.1f, 0.1f };

    [SerializeField] AudioClip[] stateLoopClip = default;
    [SerializeField] float[]     playTimeLoop = { 0, 1.2f, 1.2f, 1.2f };
    [SerializeField] float[]     pitchLoop = { 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] float[]     volumeLoop = { 0, 0.1f, 0.1f, 0.1f };
    [SerializeField] float[]     breathEndWait = default;
    [SerializeField] float       pitchBand = 0.02f;

    State prevState = State.Normal;
    float prevPlayTime;
    bool isBreathHold;

#if DEBUG
    [SerializeField] State debugState = State.Normal;
    [SerializeField] bool debugBreathHold = false;
    bool debugPrevBreathHold;
    State debugPrevState = State.Normal;
#endif

    private void Start()
    {
        source.volume = 0;
        prevPlayTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        // 息残量で呼吸音の大きさを上げる
        // スタミナ残量で管理
        // 息残量で管理
        // 両方やって大きいほうを採用
        // 自分でステート作っちゃえ
        if (!isBreathHold && currentState != State.Normal && !source.isPlaying)
        {
            int i = (int)currentState;
            if (Time.timeSinceLevelLoad - prevPlayTime > playTimeLoop[i])
            {
                PlayBreathLoopSound();
            }
        }
    }

    /// <summary>
    /// ステート変更通知
    /// </summary>
    void ChangeState(State state)
    {
        prevState = currentState;
        currentState = state;
    }

    /// <summary>
    /// 通常時の状態変化時にサウンド再生
    /// </summary>
    void PlayStateChangeSound()
    {
        // 息止め時に読んでたらエラー
        if(isBreathHold)
        {
            Debug.LogError("息止め中に呼ぶな");
            return;
        }
        switch (currentState)
        {
            // 息が粗い状態から戻る場合は終了サウンド流す
            case State.Normal:
                if (prevState == State.Middle)
                {
                    PlayBreathHoldEndSound();   // 息止め終了と同じ音流す
                    prevPlayTime = float.MaxValue;  // とりあえず再生まで無限に
                }
                break;

            // それ以外はループサウンド流す。いったん状態変更時はウェイト無しで即流し
            case State.Middle:
            case State.High:
            case State.VeryHigh:
                PlayBreathLoopSound();
                break;
        }
    }

    /// <summary>
    /// 息止め開始
    /// </summary>
    public void OnStartBreathHold()
    {
        isBreathHold = true;
        StartCoroutine(PlayBreathHoldStartSoundCoroutine());
    }

    /// <summary>
    /// 息止め終了
    /// </summary>
    public void OnEndBreathHold()
    {
        prevPlayTime = Time.timeSinceLevelLoad;
        isBreathHold = false;
        StartCoroutine(PlayBreathHoldEndSoundCoroutine());
    }

    /// <summary>
    /// 息止め開始サウンド待ちコルーチン
    /// </summary>
    IEnumerator PlayBreathHoldStartSoundCoroutine()
    {
        prevPlayTime = float.MaxValue;  // とりあえず再生まで無限に
        int i = (int)currentState;
        source.clip = holdClip;
        source.pitch = holdPitch + Random.Range(-pitchBand, pitchBand);
        source.volume = holdVolume[i];
        source.Play();
        while(source.isPlaying)
        {
            yield return null;
        }

        // TODO:必要なら苦しそうな声
    }

    /// <summary>
    /// 終了サウンドを流したのち息切れ再生開始するコルーチン
    /// </summary>
    IEnumerator PlayBreathHoldEndSoundCoroutine()
    {
        // まず息止め終了サウンド流す
        PlayBreathHoldEndSound();
        prevPlayTime = float.MaxValue;  // とりあえず再生まで無限に

        while (source.isPlaying)
        {
            yield return null;
        }

        // 流し終わったら息切れサウンド再生開始
        prevPlayTime = Time.timeSinceLevelLoad - playTimeLoop[(int)currentState] + breathEndWait[(int)currentState]; // 待ち時間固定
    }

    /// <summary>
    /// 状況に応じてループ用サウンド流す
    /// </summary>
    void PlayBreathLoopSound()
    {
        int i = (int)currentState;
        source.clip = stateLoopClip[i];
        source.pitch = pitchLoop[i] + Random.Range(-pitchBand, pitchBand);
        source.volume = volumeLoop[i];
        source.Play();
        prevPlayTime = Time.timeSinceLevelLoad;
    }

    /// <summary>
    /// 状況に応じて息止め終了用サウンド流す
    /// </summary>
    void PlayBreathHoldEndSound()
    {
        int i = (int)currentState;
        source.clip = breathEndClip[i];
        source.pitch = pitchEnd[i] + Random.Range(-pitchBand, pitchBand);
        source.volume = volumeEnd[i];
        source.Play();
    }

#if UNITY_EDITOR && DEBUG
    void OnValidate()
    {
        if(debugState != debugPrevState)
        {
            ChangeState(debugState);
            debugPrevState = debugState;
            if(!isBreathHold)
            {
                PlayStateChangeSound();
            }
        }
        if(!debugPrevBreathHold && debugBreathHold)
        {
            OnStartBreathHold();
        }
        else if (debugPrevBreathHold && !debugBreathHold)
        {
            OnEndBreathHold();
        }
        debugPrevBreathHold = debugBreathHold;
    }
#endif
}
