using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class ShadowStateFighting : StateBase
{
    // 待機カウンター
    float waitCounter = 0;
    // プレイヤーを見つけているか
    bool isDetactedPlayer = true;

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        // 初期化
        waitCounter = 0;
        isDetactedPlayer = true;

        // 移動速度を設定
        agent.speed = parameter.FightingMoveSpeed;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        // 目標位置についた
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            // 待機中
            if (animator.GetBool("IsWaiting"))
            {
                waitCounter += Time.deltaTime;
                // しばらく待機したら解除
                if (waitCounter > parameter.AttackedWaitTime)
                {
                    // 初期化
                    animator.SetBool("IsWaiting", false);
                    waitCounter = 0;

                    // 待機中にも関わらず待機時間が終了し、目的位置が更新されていない状態なので警戒に戻る
                    SetNextState((int)StateType.Caution);

                    // 攻撃したら消えるタイプ
                    if (parameter.IsAttackedDisappear)
                    {
                        // 消してリスポーン
                        animator.gameObject.SetActive(false);
                        animator.gameObject.SetActive(parameter.IsRespawn ? true : false);

                        // NOTE: yui-t SetActive(false)じゃまずいけどモックだからいいか…
                    }
                }
            }
            // 攻撃範囲内に来たら攻撃
            else if ( animator.GetBool("InAttackRange"))
            {
                // 攻撃トリガーをセット
                animator.SetTrigger("Attaking");
                // 待機フラグを立てる
                animator.SetBool("IsWaiting", true);
            }
        }
    }

    /// <summary>
    /// 終了
    /// </summary>
    public override void Exit()
    {
        // 待機を解除する
        animator.SetBool("IsWaiting", false);
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
        // プレイヤーを見つけていなければ音のほうへ向かう
        if (!isDetactedPlayer)
        {
            UpdateAimPosition(noise.transform.position);
        }
    }

    /// <summary>
    /// プレイヤーを発見した
    /// </summary>
    public override void OnDetectedPlayer(GameObject player)
    {
        isDetactedPlayer = true;
        UpdateAimPosition(player.transform.position);
    }

    /// <summary>
    ///  プレイヤーを見失った
    /// </summary>
    public override void OnMissingPlayer(GameObject player)
    { 
        isDetactedPlayer = false;
    }

    /// <summary>
    /// 目標位置更新
    /// </summary>
    void UpdateAimPosition(Vector3 aimPos)
    {
        waitCounter = 0;
        animator.SetBool("IsWaiting", false);
        agent.SetDestination(aimPos);
    }
}
