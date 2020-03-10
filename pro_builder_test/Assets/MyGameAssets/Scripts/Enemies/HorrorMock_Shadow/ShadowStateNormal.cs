using UnityEngine;
using StateType = EnemyParameter.StateType;
using NormalStateType = EnemyParameter.NormalStateType;
using WandererType = EnemyParameter.WandererType;

public class ShadowStateNormal : StateBase
{
    // 現在のチェックポイントのID
    int currentCheckPointId = 0;
    // 猶予時間のカウンター
    float safeCounter = 0;
    // 感知した音のレベル
    float detectedNoiseLevel = 0;
    Vector3 noisePos;

    bool IsDetectedNoise = false;

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        // 初期化
        currentCheckPointId = 0;
        safeCounter = parameter.SafeTime;
        detectedNoiseLevel = 0;
        IsDetectedNoise = false;
        ForceTransparentOff = false;

        // 移動速度を設定
        agent.speed = parameter.NormalMoveSpeed;

        // 徘徊タイプの影人間である
        if (parameter.NormalState == NormalStateType.Wanderer)
        {
            // 次の移動目標位置を設定
            agent.SetDestination(GetNextTargetPoint());
        }
        else if((agent.transform.position - parameter.InitialPosition).magnitude > 0.5f)
        {
            // 移動しない人で、初期位置から離れてたら次の移動目標位置は初期位置
            agent.SetDestination(parameter.InitialPosition);
            animator.SetBool("NormalWalkBack", true);
        }
    }

    /// <summary>
    /// 終わり
    /// </summary>
    public override void Exit()
    {
        base.Exit();
        animator.SetBool("NormalWalkBack", false);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        if (IsSetedNextState) return;
        // 音を聞いている
        if (IsDetectedNoise)
        {
            // 時間を減らしていく
            safeCounter -= Time.deltaTime;

            // 現在のサウンドレベルを取得
            detectedNoiseLevel = soundSpawner.TotalSoundLevel;
            // ０以下は０にする
            if (detectedNoiseLevel <= 0) { detectedNoiseLevel = 0; }
            // サウンドレベルを丸め込む
            safeCounter -= detectedNoiseLevel * 0.1f;

            // 猶予時間が０になったら
            if (safeCounter <= 0)
            {
                // 移動目標位置を発信源に
                agent.SetDestination(noisePos);

                // 警戒に移行
                SetNextState((int)StateType.Caution);
            }
        }
        // 音を聞いていない
        else
        {
            // カウンターやレベルを初期化
            safeCounter = parameter.SafeTime;
            detectedNoiseLevel = 0;
        }

        // 徘徊タイプの影人間である
        if (parameter.NormalState == NormalStateType.Wanderer)
        {
            // 目標位置につーいた！
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                // 次の移動目標位置を設定
                agent.SetDestination(GetNextTargetPoint());
            }
        }
        else
        {
            // 歩きアニメーションをやめる
            if ((agent.transform.position - parameter.InitialPosition).magnitude < 0.5f)
            {
                animator.SetBool("NormalWalkBack", false);
            }
            // 初期回転に
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, parameter.InitialRotation, 0.1f);
        }
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
    
    /// <summary>
     /// 音を聞いた(状態によって範囲が変わる、通常の聴覚範囲)
     /// </summary>
    public override void OnHearNoise(GameObject noise)
    {
        //////////////////
        // 警戒、通常共通
        //////////////////
        if (soundSpawner.TotalSoundLevel > 0)
        {
            var randomRange = parameter.NoiseTargetPosRandomRange;
            noisePos = noise.transform.position + new Vector3(Random.Range(0, randomRange), 0, Random.Range(0, randomRange));
            // 音を聞いた
            IsDetectedNoise = true;
        }
    }
}
