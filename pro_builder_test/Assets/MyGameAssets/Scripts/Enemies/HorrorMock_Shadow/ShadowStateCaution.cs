using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class ShadowStateCaution : StateBase
{
    float waitCounter = 0;
    float waitTime;

    enum State
    {
        WAIT,
        WALK
    }
    State state = State.WAIT;

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        // 初期化
        waitCounter = 0;
        waitTime = parameter.CautionStartWaitTime;
        ForceTransparentOff = false;

        // 移動速度を設定
        agent.speed = parameter.CautionMoveSpeed;

        state = State.WAIT;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        if (IsSetedNextState) return;
        switch (state)
        {
            case State.WAIT:
                UpdateWait();
                break;
            case State.WALK:
                UpdateWalk();
                break;
        }
    }

    void UpdateWait()
    {
        animator.SetBool("IsWaiting", true);
        waitCounter += Time.deltaTime;
        // しばらく待機したら
        if (waitCounter > waitTime)
        {
            // 目標位置まで距離があれば移動
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                state = State.WALK;
            }
            // 目的地についてたら通常状態に
            else
            {
                //初期位置まで距離が一定以上離れていたら
                if ((agent.transform.position - parameter.InitialPosition).magnitude > parameter.ReturnWarpDistance)
                {
                    // 初期位置まで瞬間移動する
                    agent.Warp(parameter.InitialPosition);
                }
                // 通常状態に戻る
                SetNextState((int)StateType.Normal);
            }
        }
    }
    void UpdateWalk()
    {
        animator.SetBool("IsWaiting", false);
        // 発信源についたよ
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            // 止まって待機
            waitCounter = 0;
            waitTime = Random.Range(parameter.CautionEndWaitTimeMin, parameter.CautionEndWaitTimeMax);
            state = State.WAIT;
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
            // 移動目標位置を発信源に
            var randomRange = parameter.NoiseTargetPosRandomRange;
            var targetPos = noise.transform.position + new Vector3(Random.Range(0, randomRange), 0, Random.Range(0, randomRange));
            agent.SetDestination(targetPos);
        }
    }
}