using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStateNormal : ShadowStateBase
{
    // 現在のチェックポイントの番号
    int currentCheckPointIndex = 0;
    // 次の目標位置
    Vector3 currentCheckPoint = Vector2.zero;

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        currentCheckPointIndex = 0;
        currentCheckPoint = Vector3.zero;

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
