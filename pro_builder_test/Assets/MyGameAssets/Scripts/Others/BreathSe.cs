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
    
    [SerializeField] AudioClip   deepClip = default;
    [SerializeField] float       deepPitch = default;
    [SerializeField] float[]     deepVolume = default;
    [SerializeField] AudioClip[] deepEndClip = default;
    [SerializeField] float[]     pitchDeepEnd = { 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] float[]     volumeDeepEnd = default;

    [SerializeField] AudioClip[] stateLoopClip = default;
    [SerializeField] float[]     playTimeLoop = { 0, 1.2f, 1.2f, 1.2f };
    [SerializeField] float[]     pitchLoop = { 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] float[]     volumeLoop = { 0, 0.1f, 0.1f, 0.1f };
    [SerializeField] float[]     breathEndWait = default;
    [SerializeField] float       pitchBand = 0.02f;

    [SerializeField] PlayerBreathController  breathController;
    [SerializeField] playerStaminaController staminaController;
    [SerializeField] PlayerEvents            playerEvent;
    [SerializeField] PlayerStatusData        playerData;

    State prevState = State.Normal;
    float prevPlayTime;
    bool isBreathHold;
    bool isDeepBreath;
    IEnumerator playStateChangeSoundCoroutine;

#if DEBUG
    [SerializeField] State debugState = State.Normal;
    [SerializeField] bool debugBreathHold = false;
    bool debugPrevBreathHold;
    [SerializeField] bool debugDeepBreath = false;
    bool debugPrevDeepBreath;
    State debugPrevState = State.Normal;
#endif

    private void Start()
    {
        source.volume = 0;
        prevPlayTime = Time.timeSinceLevelLoad;
        playStateChangeSoundCoroutine = PlayStateChangeSoundCoroutine();
    }

    private void Update()
    {
        if (!isBreathHold && playerEvent.IsBreathHold)
        {
            OnStartBreathHold();
        }
        else if (isBreathHold && !playerEvent.IsBreathHold)
        {
            OnEndBreathHold();
        }
        if (!isDeepBreath && playerEvent.IsDeepBreath)
        {
            OnStartDeepBreath();
        }
        else if (isDeepBreath && !playerEvent.IsDeepBreath)
        {
            OnEndDeepBreath();
        }

        State nextState = currentState;
        // 息残量で管理
        if (breathController.NowAmount < playerData.LargeDisturbance)
        {
            nextState = State.VeryHigh;
        }
        else if (breathController.NowAmount < playerData.MediumDisturbance)
        {
            nextState = State.High;
        }
        else if (breathController.NowAmount < playerData.SmallDisturbance)
        {
            nextState = State.Middle;
        }
        else
        {
            nextState = State.Normal;
        }

        //// スタミナ残量で管理
        //if (staminaController.NowAmount < playerData.LargeStaminaDisturbance)
        //{
        //    nextState = State.VeryHigh;
        //}
        //if (staminaController.NowAmount < playerData.MediumStaminaDisturbance)
        //{
        //    nextState = State.High;
        //}
        //if (staminaController.NowAmount < playerData.SmallStaminaDisturbance)
        //{
        //    nextState = State.Middle;
        //}
        //else
        //{
        //    nextState = State.Normal;
        //}
        // 両方やって大きいほうを採用
        if ((int)nextState < (int)State.VeryHigh && staminaController.NowAmount < playerData.LargeStaminaDisturbance)
        {
            nextState = State.VeryHigh;
        }
        else if ((int)nextState < (int)State.High && staminaController.NowAmount < playerData.MediumStaminaDisturbance)
        {
            nextState = State.High;
        }
        else if ((int)nextState < (int)State.Middle && staminaController.NowAmount < playerData.SmallStaminaDisturbance)
        {
            nextState = State.Middle;
        }
        if(currentState != nextState)
        {
            ChangeState(nextState);
            if (!isBreathHold && !isDeepBreath)
            {
                PlayStateChangeSound();
            }
        }

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
        if (currentState != state)
        {
            prevState = currentState;
            currentState = state;
        }
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

            case State.Middle:
            case State.High:
            case State.VeryHigh:
                StopCoroutine(playStateChangeSoundCoroutine);
                StartCoroutine(playStateChangeSoundCoroutine);
                break;
        }
    }
    IEnumerator PlayStateChangeSoundCoroutine()
    {
        prevPlayTime = float.MaxValue;  // とりあえず再生まで無限に
        while (source.isPlaying)
        {
            yield return null;
        }
        PlayBreathLoopSound();
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

        // ループ時間分は待ってから切り替えてサウンド出す
        prevPlayTime = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - prevPlayTime > playTimeLoop[(int)currentState])
        {
            yield return null;
        }
        prevPlayTime = Time.timeSinceLevelLoad - playTimeLoop[(int)currentState] + breathEndWait[(int)currentState]; // 待ち時間固定
    }

    /// <summary>
    /// 深呼吸開始
    /// </summary>
    public void OnStartDeepBreath()
    {
        isDeepBreath = true;
        prevPlayTime = float.MaxValue;  // とりあえず再生まで無限に
        int i = (int)currentState;
        source.clip = deepClip;
        source.pitch = deepPitch + Random.Range(-pitchBand, pitchBand);
        source.volume = deepVolume[i];
        source.Play();
    }

    /// <summary>
    /// 深呼吸終了
    /// </summary>
    public void OnEndDeepBreath()
    {
        prevPlayTime = Time.timeSinceLevelLoad;
        isDeepBreath = false;
        StartCoroutine(PlayDeepBreathEndSoundCoroutine());
    }
    

    /// <summary>
    /// 深呼吸終了サウンドを流したのち息切れ再生開始するコルーチン
    /// </summary>
    IEnumerator PlayDeepBreathEndSoundCoroutine()
    {
        // まず深呼吸終了サウンド流す
        int i = (int)currentState;
        source.clip = deepEndClip[i];
        source.pitch = pitchDeepEnd[i] + Random.Range(-pitchBand, pitchBand);
        source.volume = volumeDeepEnd[i];
        source.Play();

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
            if(!isBreathHold && !isDeepBreath)
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
        if (!debugPrevDeepBreath && debugDeepBreath)
        {
            OnStartDeepBreath();
        }
        else if (debugPrevDeepBreath && !debugDeepBreath)
        {
            OnEndDeepBreath();
        }
        debugPrevBreathHold = debugBreathHold;
        debugPrevDeepBreath = debugDeepBreath;
    }
#endif
}
