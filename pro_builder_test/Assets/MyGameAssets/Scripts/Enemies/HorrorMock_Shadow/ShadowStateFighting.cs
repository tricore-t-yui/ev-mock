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
        ATTACK_WAIT
    }
    State state = State.RUN;
    Vector3 playerPos;

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        // 初期化
        waitCounter = 0;
        isDetectedPlayer = true;
        state = State.RUN;

        // 既にみつけているので、プレイヤーの位置を目標位置に設定済
        //agent.SetDestination(player.transform.position);

        // 移動速度を設定
        agent.speed = parameter.FightingMoveSpeed;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        if (IsSetedNextState) return;
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
        }
    }

    void UpdateRunState()
    {
        animator.SetBool("IsWaiting", false);
        agent.updatePosition = true;
        agent.updateRotation = true;
        UpdateViewTargetPos();

        // 攻撃目標位置についた
        if (agent.remainingDistance <= parameter.AttackRange)
        {
            // 攻撃範囲内にいたら攻撃
            if (IsInAttackRange)
            {
                // 攻撃トリガーをセット
                animator.SetTrigger("Attaking");
                state = State.ATTACK;
            }
            else
            {
                // 更新して攻撃範囲にプレイヤーがいないということは見失っているので警戒に戻る
                SetNextState((int)StateType.Caution);
                // 初期化
                waitCounter = 0;
            }
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
        state = State.ATTACK_WAIT;
    }

    void UpdateAttackWaitState()
    {
        // 待機フラグを立てる
        animator.SetBool("IsWaiting", true);

        agent.updatePosition = false;
        agent.updateRotation = true;
        // 待機中
        UpdateViewTargetPos();  // 待機中もプレイヤーの位置の補足は更新し続ける
        waitCounter += Time.deltaTime;
        // しばらく待機したら解除
        if (waitCounter > parameter.AttackedWaitTime)
        {
            waitCounter = 0;
            state = State.RUN;
        }
    }

    /// <summary>
    /// 視線情報によるターゲットポジション更新
    /// </summary>
    void UpdateViewTargetPos()
    {
        // プレイヤーを見つけていればプレイヤー位置
        // 見失っていれば最後にプレイヤーを見た位置
        // ノイズ位置はプレイヤーが見つかっていない状態で音を聞けば即座に更新される
        if(isDetectedPlayer)
        {
            agent.SetDestination(playerPos);
        }
    }

    /// <summary>
    /// 音を聞いた(状態によって範囲が変わる、通常の聴覚範囲)
    /// </summary>
    public override void OnHearNoise(GameObject noise)
    {
        // 戦闘状態のほうにやらせるのでなにもしない
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
            agent.SetDestination(noisePos);
        }
    }

    /// <summary>
    /// プレイヤーを発見した
    /// </summary>
    public override void OnDetectedPlayer(GameObject player)
    {
        isDetectedPlayer = true;
        playerPos = player.transform.position;
    }

    /// <summary>
    /// プレイヤーを発見している視線ループ
    /// </summary>
    public override void OnDetectPlayerStay(GameObject player)
    {
        playerPos = player.transform.position;
    }

    /// <summary>
    ///  プレイヤーを見失った
    /// </summary>
    public override void OnMissingPlayer(GameObject player)
    { 
        isDetectedPlayer = false;
        playerPos = player.transform.position;
    }
}
