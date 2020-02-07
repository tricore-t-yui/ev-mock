using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowState = ShadowParameter.StateType;

public class ShadowStateAlert : ShadowStateBase
{
    SoundAreaSpawner soundSpawner = default;
    float alertTimeCounter = 0;
    float currentSoundLevel = 0;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="sound"></param>
    public ShadowStateAlert(SoundAreaSpawner sound)
    {
        soundSpawner = sound;
    }

    public override void Entry()
    {
        alertTimeCounter = parameter.AlertTime;

        // 辺りに注意しているときの待機フラグをオンにする
        animator.SetBool("IsWaiting", true);
    }

    public override void Update()
    {
        // 時間を減らしていく
        alertTimeCounter -= Time.deltaTime;

        // 現在のサウンドレベルを取得
        currentSoundLevel = soundSpawner.TotalSoundLevel;
        // ０以下は０にする
        if (currentSoundLevel <= 0) { currentSoundLevel = 0; }

        if (currentSoundLevel > parameter.SafeSoundLevelMax)
        {
            // 警戒に移行
            SetNextState((int)ShadowState.Caution);
            animator.SetBool("IsWaiting", false);
        }

        // サウンドレベルを丸め込む
        alertTimeCounter -= currentSoundLevel * 0.1f;

        // 猶予時間が０になったら
        if (alertTimeCounter <= 0)
        {
            // 警戒に移行
            SetNextState((int)ShadowState.Caution);
            animator.SetBool("IsWaiting", false);
        }
    }

    public override void Exit()
    {
        
    }
}
