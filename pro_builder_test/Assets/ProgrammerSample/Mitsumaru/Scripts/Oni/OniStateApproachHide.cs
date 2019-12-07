using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ハイドポイントまで接近
/// </summary>
public class OniStateApproachHide : StateMachineBehaviour
{
    [SerializeField]
    float nearDistance = 0;

    OniViewingRay viewingRay = null;

    // プレイヤーのハイドコントローラー
    PlayerHideController playerHideController = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // プレイヤーのハイドコントローラーを取得
        playerHideController = FindObjectOfType<PlayerHideController>() ?? playerHideController;

        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        viewingRay = FindObjectOfType<OniViewingRay>() ?? viewingRay;

        if (navMesh.remainingDistance < nearDistance)
        {
            animator.SetBool("isNearTheHideMove", true);
        }
    }
}
