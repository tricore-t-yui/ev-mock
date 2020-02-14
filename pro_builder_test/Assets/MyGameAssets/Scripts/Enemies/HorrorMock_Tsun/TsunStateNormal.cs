using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;
using NormalStateType = EnemyParameter.NormalStateType;
using WandererType = EnemyParameter.WandererType;

public class TsunStateNormal : StateBase
{
    // 現在のチェックポイントのID
    int currentCheckPointId = 0;
    // 猶予時間のカウンター
    float safeCounter = 0;
    // 感知した音のレベル
    float detectedNoiseLevel = 0;
    // サウンドスポナー
    SoundAreaSpawner soundSpawner = default;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="sound"></param>
    public TsunStateNormal(SoundAreaSpawner sound)
    {
        soundSpawner = sound;
    }

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        // 初期化
        currentCheckPointId = 0;
        safeCounter = parameter.SafeTime;
        detectedNoiseLevel = 0;

        // 移動速度を設定
        agent.speed = parameter.NormalMoveSpeed;

        // 徘徊タイプの影人間である
        if (parameter.NormalState == NormalStateType.Wanderer)
        {
            // 次の移動目標位置を設定
            agent.SetDestination(GetNextTargetPoint());
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        // 影人間が出現している
        if (animator.GetBool("IsDetectedNoise"))
        {
            // 時間を減らしていく
            safeCounter -= Time.deltaTime;

            // 現在のサウンドレベルを取得
            detectedNoiseLevel = soundSpawner.TotalSoundLevel;
            // ０以下は０にする
            if (detectedNoiseLevel <= 0) { detectedNoiseLevel = 0; }

            if (detectedNoiseLevel > parameter.SafeSoundLevelMax)
            {
                // 警戒に移行
                SetNextState((int)StateType.Caution);
            }

            // サウンドレベルを丸め込む
            safeCounter -= detectedNoiseLevel * 0.1f;

            // 猶予時間が０になったら
            if (safeCounter <= 0)
            {
                // 警戒に移行
                SetNextState((int)StateType.Caution);
            }
        }
        // 出現していないよ
        else
        {
            // カウンターやレベルを初期化
            safeCounter = parameter.SafeTime;
            detectedNoiseLevel = 0;
        }

        // 徘徊タイプのツンである
        if (parameter.NormalState == NormalStateType.Wanderer)
        {
            // 目標位置につーいた！
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                // 次の移動目標位置を設定
                agent.SetDestination(GetNextTargetPoint());
            }
        }
    }

    /// <summary>
    /// 終了
    /// </summary>
    public override void Exit()
    {
        
    }

    /// <summary>
    /// 次の移動目標位置を取得
    /// </summary>
    /// <returns></returns>
    Vector3 GetNextTargetPoint()
    {
        // ルート
        if (parameter.Wanderer == WandererType.Route)
        {
            currentCheckPointId++;
            if (currentCheckPointId > parameter.RouteCheckPoints.Count - 1) { currentCheckPointId = 0; }
            return parameter.RouteCheckPoints[currentCheckPointId];
        }
        // ランダム
        else
        {
            float radius = Random.Range(parameter.RandomRangeRadiusMin, parameter.RandomRangeRadiusMax);
            float angle = Random.Range(0.0f, Mathf.PI * 2);
            return parameter.InitialPosition + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        }
    }
}
