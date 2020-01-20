using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// プレイヤーまで接近
/// </summary>
public class OniStateApproachThePlayer : StateMachineBehaviour
{
    enum PlayerConditionKind
    {
        Normal,
        HideInLocker,
        HideInBed,
        HideInDuct,
        Num,
    }

    // 現在のプレイヤーの態勢
    PlayerConditionKind currentPlayerCondition = PlayerConditionKind.Normal;

    // 状態ごとの攻撃可能距離
    [SerializeField]
    float[] attackPossibleDistance = new float[(int)PlayerConditionKind.Num];
    // 攻撃可能距離にいるかどうか
    bool isAttackPossibleDistance = false;

    // プレイヤー
    GameObject player = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // プレイヤーのハイドコントローラー
    PlayerHideController playerHideController = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // プレイヤーを取得
        player = GameObject.FindGameObjectWithTag("Player") ?? player;

        // ナビメッシュの取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
        navMesh.SetDestination(player.transform.position);

        // プレイヤーのハイドコントローラーを取得
        playerHideController = FindObjectOfType<PlayerHideController>() ?? playerHideController;
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュの移動速度をセット
        navMesh.speed = animator.GetFloat("moveSpeed");
        // 移動目標位置をセット
        navMesh.SetDestination(player.transform.position);

        // プレイヤーが隠れていたら
        if (animator.GetBool("isPlayerHide"))
        {
            // ロッカー
            if (playerHideController.IsHideLocker)
            {
                currentPlayerCondition = PlayerConditionKind.HideInLocker;
            }
            // ベッド
            else if (playerHideController.IsHideBed)
            {
                currentPlayerCondition = PlayerConditionKind.HideInBed;
            }
            // ダクト
            else if (playerHideController.IsDuctArea)
            {
                currentPlayerCondition = PlayerConditionKind.HideInDuct;
            }
        }
        else
        {
            // 通常（隠れていない）
            currentPlayerCondition = PlayerConditionKind.Normal;
        }

        // プレイヤーとの距離を算出
        float distance = (player.transform.position - animator.transform.position).magnitude;
        
        // 攻撃可能距離に達したら
        if (distance < attackPossibleDistance[(int)currentPlayerCondition])
        {
            if (!isAttackPossibleDistance)
            {
                // 攻撃可能トリガーをセット
                animator.SetTrigger("attackStart");
                // プレイヤー側の状態をセット
                animator.SetInteger("playerConditionKindId", (int)currentPlayerCondition);
                // 攻撃可能距離に達したフラグをセット
                isAttackPossibleDistance = true;
            }
        }
        else
        {
            // 攻撃可能距離の範囲外にでたらフラグをリセット
            isAttackPossibleDistance = false;
        }
    }
}
