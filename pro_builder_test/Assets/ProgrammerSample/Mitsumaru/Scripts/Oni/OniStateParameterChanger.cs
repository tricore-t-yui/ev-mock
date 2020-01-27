﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鬼の状態ごとのパラメータ
/// </summary>
[System.Serializable]
class OniParameter
{
    public float moveSpeed;
    public int searchingTime;
    public float searchingRange;
    public int rayIrradiationTimeToFighting;
    public int rayBlockingTimeToVigilance;
}

/// <summary>
/// 状態によって鬼のパラメータを変更する
/// </summary>
public class OniStateParameterChanger : StateMachineBehaviour
{
    // 鬼の状態の種類
    public enum StateKind
    {
        Vigilance,   // 警戒状態
        Fighting,    // 戦闘状態
        Num,         // 番兵
    }

    // プレイヤーのハイドコントローラー
    PlayerHideController playerHideController = null;

    // 鬼の視野のレイ
    OniViewingRay viewingRay = null;

    // 鬼の聴覚範囲コライダーのイベント
    ColliderEvent noiseListenerColliderEvent = null;

    // 状態ごとの鬼のパラメータ
    [SerializeField]
    OniParameter[] parameters = new OniParameter[(int)StateKind.Num];

    // 敵のサウンドプレイヤー取得
    EnemySoundPlayer soundPlayer = null;

    // レイの照射時間
    int rayIrradiationCounter = 0;
    // レイの遮断時間
    int rayBlockingTimeToVigilance = 0;

    // 音が聞こえたかどうか
    bool isNoiseListener = false;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 鬼のレイを取得
        viewingRay = FindObjectOfType<OniViewingRay>() ?? viewingRay;

        // プレイヤーのハイドコントローラーを取得
        playerHideController = FindObjectOfType<PlayerHideController>() ?? playerHideController;

        // 前回のステートとしてセット
        animator.SetInteger("prevStateKindId", (int)StateKind.Vigilance);
        // 現在のステートのIDをセット
        animator.SetInteger("currentStateKindId", (int)StateKind.Vigilance);

        // サウンドプレイヤー
        soundPlayer = animator.GetComponentInChildren<EnemySoundPlayer>() ?? soundPlayer;
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ステートのIDを取得
        int currentStateId = animator.GetInteger("oniStateKindId");

        // 各パラメータを状態によって切り替える
        animator.SetFloat("moveSpeed", parameters[currentStateId].moveSpeed);
        animator.SetInteger("searchingTime", parameters[currentStateId].searchingTime);
        animator.SetFloat("searchingRange", parameters[currentStateId].searchingRange);
        animator.SetInteger("rayIrradiationTimeToFighting", parameters[currentStateId].rayIrradiationTimeToFighting);
        animator.SetInteger("rayBlockingTimeToVigilance", parameters[currentStateId].rayBlockingTimeToVigilance);

        // 前回のステートとしてセット
        animator.SetInteger("prevStateKindId", animator.GetInteger("currentStateKindId"));
        // 現在のステートのIDをセット
        animator.SetInteger("currentStateKindId",currentStateId);

        //Debug.Log(playerHideController.IsDuctArea);

        // プレイヤーがハイドポイントに入ったら
        if (playerHideController.IsHideLocker || playerHideController.IsHideBed || playerHideController.IsDuctArea)
        {
            // ステートを変更
            animator.SetBool("isPlayerHide", true);

            // レイが一定時間照射され続けていたら
            if (rayIrradiationCounter > parameters[currentStateId].rayIrradiationTimeToFighting)
            {
                // プレイヤーを発見して、戦闘状態へ
                animator.SetBool("isPlayerDiscover", true);
                animator.SetInteger("oniStateKindId", (int)StateKind.Fighting);
            }

            // 鬼のレイが当たり続けている間
            if (viewingRay?.HitObject?.tag == "Player")
            {
                rayBlockingTimeToVigilance = 0;
                rayIrradiationCounter++;
            }
        }
        // ハイドポイントに隠れていない
        else
        {
            // ステートを変更
            animator.SetBool("isPlayerHide", false);

            // レイがプレイヤーに当たったら
            if (viewingRay?.HitObject?.tag == "Player")
            {
                // プレイヤーを発見して、戦闘状態へ
                animator.SetInteger("oniStateKindId", (int)StateKind.Fighting);
                animator.SetBool("isPlayerDiscover", true);
            }

            if (rayBlockingTimeToVigilance > 30)
            {
                // プレイヤーを発見して、戦闘状態へ
                animator.SetBool("isPlayerDiscover", false);
                animator.SetInteger("oniStateKindId", (int)StateKind.Vigilance);
            }
        }

        // プレイヤーの物音を聴いたら
        if (animator.GetBool("isNoiseListener"))
        {
            // ハイドポイントに隠れいている場合
            if (animator.GetBool("isPlayerHide"))
            {
                // プレイヤーを発見して、戦闘状態へ
                animator.SetInteger("oniStateKindId", (int)StateKind.Fighting);
                animator.SetBool("isPlayerDiscover", true);
            }
            else
            {
                // 戦闘状態に切り替える
                animator.SetInteger("oniStateKindId", (int)StateKind.Fighting);
            }
        }

        // 鬼のレイが当たり続けている間
        if (viewingRay?.HitObject?.tag == "Player")
        {
            rayBlockingTimeToVigilance = 0;
            rayIrradiationCounter++;
        }
        // レイが遮断され続けている間
        else
        {
            rayIrradiationCounter = 0;
            rayBlockingTimeToVigilance++;
        }
        // 鬼のレイの遮断時間が一定時間経過したら
        if (rayBlockingTimeToVigilance > parameters[currentStateId].rayBlockingTimeToVigilance)
        {
            // 警戒状態に戻す
            animator.SetInteger("oniStateKindId", (int)StateKind.Vigilance);
            animator.SetBool("isPlayerDiscover", false);
        }
    }
}
