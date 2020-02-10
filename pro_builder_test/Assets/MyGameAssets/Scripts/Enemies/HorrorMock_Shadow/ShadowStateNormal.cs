using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowState = ShadowParameter.StateType;

public class ShadowStateNormal : ShadowStateBase
{
    // 現在のチェックポイントの番号
    int currentCheckPointIndex = 0;
    // 次の目標位置
    Vector3 currentCheckPoint = Vector2.zero;
    float alertTimeCounter = 0;
    float currentSoundLevel = 0;
    float appearFadeCounter = 0;
    SoundAreaSpawner soundSpawner = default;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="sound"></param>
    public ShadowStateNormal(SoundAreaSpawner sound)
    {
        soundSpawner = sound;
    }

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        appearFadeCounter = 1;
        currentCheckPointIndex = 0;
        currentCheckPoint = Vector3.zero;

        meshRenderer.enabled = false;

        // 移動速度を設定
        agent.speed = parameter.NormalMoveSpeed;

        // 初期チェックポイント設定
        if (parameter.NormalState != ShadowParameter.NormalStateType.Wanderer) { return; }
        // ルート
        if (parameter.Wanderer == ShadowParameter.WandererType.Route)
        {
            currentCheckPoint = parameter.RouteCheckPoints[currentCheckPointIndex];
            agent.SetDestination(new Vector3(currentCheckPoint.x, agent.transform.position.y, currentCheckPoint.y));
        }
        // ランダム
        else
        {
            // 目標位置を設定
            GetRandomTargetPos();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        if (animator.GetBool("IsCautionEnter"))
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
            }

            // サウンドレベルを丸め込む
            alertTimeCounter -= currentSoundLevel * 0.1f;

            // 猶予時間が０になったら
            if (alertTimeCounter <= 0)
            {
                // 警戒に移行
                SetNextState((int)ShadowState.Caution);
            }
        }

        // それぞれのタイプで移動を行う
        if (parameter.Wanderer == ShadowParameter.WandererType.Route) { RouteType();  }
        else                                                          { RandomType(); }
    }

    /// <summary>
    /// 終了
    /// </summary>
    public override void Exit()
    {

    }

    /// <summary>
    /// ルートタイプ
    /// </summary>
    public void RouteType()
    {
        // 到着したら
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            // 初期チェックポイント設定
            currentCheckPointIndex++;
            if (currentCheckPointIndex > parameter.RouteCheckPoints.Count-1) { currentCheckPointIndex = 0; }
            currentCheckPoint = parameter.RouteCheckPoints[currentCheckPointIndex];
            agent.SetDestination(new Vector3(currentCheckPoint.x, agent.transform.position.y, currentCheckPoint.y));
        }
    }

    /// <summary>
    /// ランダムタイプ
    /// </summary>
    public void RandomType()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            // 目標位置を設定
            GetRandomTargetPos();
        }
    }

    public void GetRandomTargetPos()
    {
        // コライダーの範囲内で目標位置を取得
        float radius = Random.Range(parameter.RandomRangeRadiusMin, parameter.RandomRangeRadiusMax);
        float angle = Random.Range(0.0f, Mathf.PI * 2);
        currentCheckPoint = parameter.InitialPosition + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        agent.SetDestination(currentCheckPoint);
    }
}
