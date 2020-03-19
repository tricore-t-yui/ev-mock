using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class ShadowStateFighting : StateBase
{
    // 待機カウンター
    float waitCounter = 0;
    // プレイヤーを見つけているか
    bool isDetectedPlayer = true;
    
    enum State
    {
        RUN,
        ATTACK,
        ATTACK_WAIT,
        //ATTACK_POS_ADJUST,
    }
    State state = State.RUN;
    Vector3 playerPos;
    protected Vector3 currentTargetPos;

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        // 初期化
        waitCounter = 0;
        isDetectedPlayer = true;
        state = State.RUN;

        // 見つかっているので強制不透明に
        ForceTransparentOff = true;

        // 既にみつけているので、プレイヤーの位置を目標位置に設定済
        playerPos = agent.destination;
        currentTargetPos = playerPos;
        //SetSafeDestination(player.transform.position);

        // 移動速度を設定
        agent.speed = parameter.FightingMoveSpeed;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        if (IsSetedNextState) return;
        // 息止めされたら見失う
        if(!parameter.IsAbleDetectBreathHoldPlayer && enemy.PlayerState.IsBreathHold)
        {
            isDetectedPlayer = false;

            // ウェイト中なら即座にRUNに
            if (state == State.ATTACK_WAIT)
            {
                state = State.RUN;
                waitCounter = 0;
            }
        }

        UpdateRotation(currentTargetPos);
        agent.SetDestination(currentTargetPos);

        switch (state)
        {
            case State.RUN:
                UpdateRunState();
                break;
            case State.ATTACK:
                break;
            case State.ATTACK_WAIT:
                UpdateAttackWaitState();
                break;
            //case State.ATTACK_POS_ADJUST:
            //    UpdateAttackPosAdjustState();
            //    break;
        }
    }

    void UpdateRunState()
    {
        animator.SetBool("IsWaiting", false);

        // 攻撃目標位置についた
        if (agent.remainingDistance <= parameter.AttackRange)
        {
            agent.isStopped = true;
            // 攻撃範囲内にいたら攻撃
            if (enemy.IsInAttackRange && isDetectedPlayer)
            {
                // 攻撃しない状態ならそのまま待機へ
                if (parameter.DontAttack)
                {
                    state = State.ATTACK_WAIT;
                }
                else
                {
                    // 攻撃トリガーをセット
                    animator.SetTrigger("Attaking");
                    state = State.ATTACK;
                }
            }
            else if(!isDetectedPlayer)
            {
                // 更新して攻撃範囲にプレイヤーがいなくて見失っているので警戒に戻る
                SetNextState((int)StateType.Caution);
                // 初期化
                waitCounter = 0;
            }
        }
    }

    //void UpdateAttackPosAdjustState()
    //{
    //    animator.SetBool("IsWaiting", false);
    //    agent.SetDestination(currentTargetPos);

    //    // 攻撃目標位置についた
    //    if (agent.remainingDistance <= parameter.AttackRange)
    //    {
    //        agent.isStopped = true;
    //        // 攻撃範囲内にいたら攻撃
    //        if (enemy.IsInAttackRange)
    //        {
    //            // 攻撃しない状態ならそのまま待機へ
    //            if (parameter.DontAttack)
    //            {
    //                state = State.ATTACK_WAIT;
    //            }
    //            else
    //            {
    //                // 攻撃トリガーをセット
    //                animator.SetTrigger("Attaking");
    //                state = State.ATTACK;
    //            }
    //        }
    //        else
    //        {
    //            // 最後に見たプレイヤー位置に素早く向き直る
    //            UpdateRotation(playerPos);
    //            // 初期化
    //            waitCounter = 0;
    //        }
    //    }
    //    else
    //    {
    //        UpdateRotation(agent.destination);
    //    }
    //}

    void UpdateAttackWaitState()
    {
        // 待機フラグを立てる
        animator.SetBool("IsWaiting", true);

        // 待機中
        waitCounter += Time.deltaTime;
        // しばらく待機したら解除
        if (waitCounter > parameter.AttackedWaitTime)
        {
            waitCounter = 0;
            state = State.RUN;
        }
    }

    void UpdateRotation(Vector3 target)
    {
        var toTarget = target - agent.transform.position;
        var toTargetMag = toTarget.magnitude;
        if (toTargetMag < 2.0f && toTargetMag > float.Epsilon && toTarget.normalized.sqrMagnitude > 0)
        {
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, Quaternion.LookRotation(toTarget.normalized), 0.3f);
        }
    }

    /// <summary>
    /// 攻撃実行
    /// </summary>
    public override void OnAttack()
    {
        // 攻撃したら消えるタイプなら消える
        if (parameter.IsAttackedDisappear)
        {
            // 消してリスポーン
            animator.gameObject.SetActive(false);
            animator.gameObject.SetActive(parameter.IsRespawn ? true : false);

            // NOTE: yui-t SetActive(false)じゃまずいけどモックだからいいか…
        }
        waitCounter = 0;
        state = State.ATTACK_WAIT;
    }

    /// <summary>
    /// 音を聞いた(状態によって範囲が変わる、通常の聴覚範囲)
    /// </summary>
    public override void OnHearNoise(GameObject noise)
    {
        // 戦闘状態のほうにやらせるのでなにもしない
    }
    /// <summary>
    /// 直接感知範囲で音を聞いた（状態によって変わらない、この範囲で一定以上の音を聞くと即座に攻撃に移動する範囲。通常の聴覚範囲より優先される）
    /// </summary>
    public override void OnHearNoiseAtDirectDetectRange(GameObject noise)
    {
        // 既に戦闘状態なので何もしない
    }

    /// <summary>
    /// 戦闘範囲で音を聞いた（状態によって変わらない）
    /// </summary>
    public override void OnHearNoiseAtFightingRange(GameObject noise)
    {
        // プレイヤーが見つかっていない状態で音を聞けば即座に更新される
        if (!isDetectedPlayer)
        {
            var randomRange = parameter.NoiseTargetPosRandomRange;
            var noisePos = noise.transform.position + new Vector3(Random.Range(0, randomRange), 0, Random.Range(0, randomRange));
            currentTargetPos = noisePos;
        }
    }

    /// <summary>
    /// プレイヤーを発見した
    /// </summary>
    public override void OnDetectedPlayer(GameObject player)
    {
        isDetectedPlayer = true;
        playerPos = player.transform.position;
        currentTargetPos = playerPos;
    }

    /// <summary>
    /// プレイヤーを発見している視線ループ
    /// </summary>
    public override void OnDetectPlayerStay(GameObject player)
    {
        playerPos = player.transform.position;
        currentTargetPos = playerPos;
    }

    /// <summary>
    ///  プレイヤーを見失った
    /// </summary>
    public override void OnMissingPlayer(GameObject player)
    { 
        isDetectedPlayer = false;
        playerPos = player.transform.position;
        currentTargetPos = playerPos;
    }

    ///// <summary>
    ///// 距離が近い場合に、ぴったりその目標地点に重ならないように目標を設定する
    ///// </summary>
    //void SetAttackDestination(Vector3 dest)
    //{
    //    agent.SetDestination(dest);

    //    float limitDest = 1.4f;
    //    var srcToDest = dest - agent.transform.position;
    //    var mag = srcToDest.magnitude;
    //    // 目標がほぼぴったり同じ位置にあったら、正面に目標位置設定
    //    // ダメだったら周りのランダムな位置に目標指定
    //    // ランダム半径は少しずつ広げる
    //    // 移動距離が半径の二倍より大きい場合は適用しない
    //    bool find = false;
    //    if (mag < limitDest)
    //    {
    //        const int loopMax = 100;
    //        int loopCount = 0;
    //        Vector3 finalDest = dest;
    //        float limitDestInterval = 0.5f;
    //        const int randomCount = 5;
    //        while (loopCount < loopMax && !find)
    //        {
    //            for (int i = 0; i < randomCount; i++)
    //            {
    //                Vector3 newDest;
    //                if (i == 0)
    //                {
    //                    newDest = dest + (-srcToDest.normalized) * limitDest;
    //                }
    //                else
    //                {
    //                    var randomRangeDir = new Vector3(Mathf.Cos(Random.Range(0, Mathf.PI * 2)), 0, Mathf.Sin(Random.Range(0, Mathf.PI * 2)));
    //                    newDest = dest + randomRangeDir * limitDest;
    //                }
    //                agent.SetDestination(newDest);
    //                if (agent.remainingDistance < limitDest * 2)
    //                {
    //                    find = true;
    //                    break;
    //                }
    //            }
    //            ++loopCount;
    //        }
    //        limitDest += limitDestInterval;
    //        if (!find)
    //        {
    //            agent.SetDestination(dest);
    //            Debug.LogWarning("loop count over");
    //        }
    //    }
    //}
}
