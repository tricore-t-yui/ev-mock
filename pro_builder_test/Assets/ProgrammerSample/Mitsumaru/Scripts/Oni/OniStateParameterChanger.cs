using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class OniParameter
{
    public float moveSpeed;
    public int searchingTime;
    public float searchingRange;
    public int rayIrradiationTimeToFighting;
    public int rayBlockingTimeToVigilance;
}

public class OniStateParameterChanger : StateMachineBehaviour
{
    public enum StateKind
    {
        Vigilance,   // 警戒状態
        Fighting,    // 戦闘状態
        Num,         // 番兵
    }

    // プレイヤーのハイドコントローラー
    PlayerHideController playerHideController = null;

    OniViewingRay viewingRay = null;
    ColliderEvent noiseListenerColliderEvent = null;

    [SerializeField]
    OniParameter[] parameters = new OniParameter[(int)StateKind.Num];

    // レイの照射時間
    int rayIrradiationCounter = 0;
    // レイの遮断時間
    int rayBlockingTimeToVigilance = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        viewingRay = FindObjectOfType<OniViewingRay>() ?? viewingRay;

        // プレイヤーのハイドコントローラーを取得
        playerHideController = FindObjectOfType<PlayerHideController>() ?? playerHideController;
        // コライダーイベントを取得
        noiseListenerColliderEvent = animator.transform.Find("Collider").Find("NoiseListenerRange").GetComponent<ColliderEvent>() ?? noiseListenerColliderEvent;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ステートのIDを取得
        int currentStateId = animator.GetInteger("oniStateKindId");

        // プレイヤーがハイドポイントに入ったら
        if (playerHideController.IsHideLocker || playerHideController.IsHideBed)
        {
            // ステートを変更
            animator.SetBool("isPlayerHide", true);

            if (rayIrradiationCounter > parameters[currentStateId].rayIrradiationTimeToFighting)
            {
                animator.SetBool("isPlayerDiscover", true);
                animator.SetInteger("oniStateKindId", (int)StateKind.Fighting);
            }
        }
        else
        {
            // ステートを変更
            animator.SetBool("isPlayerHide", false);

            if (viewingRay.HitObject.tag == "Player")
            {
                animator.SetInteger("oniStateKindId", (int)StateKind.Fighting);
                animator.SetBool("isPlayerDiscover", true);
            }
        }

        if (rayBlockingTimeToVigilance > parameters[currentStateId].rayBlockingTimeToVigilance)
        {
            animator.SetInteger("oniStateKindId", (int)StateKind.Vigilance);
        }

        if (viewingRay.HitObject.tag == "Player")
        {
            rayBlockingTimeToVigilance = 0;
            rayIrradiationCounter++;
        }
        else
        {
            rayIrradiationCounter = 0;
            rayBlockingTimeToVigilance++;
        }

        rayIrradiationCounter++;
        rayBlockingTimeToVigilance++;

        if (animator.GetBool("isNoiseListener"))
        {
            if (animator.GetBool("isPlayerHide"))
            {
                animator.SetInteger("oniStateKindId", (int)StateKind.Fighting);
                animator.SetBool("isPlayerDiscover", true);
            }
            else
            {
                animator.SetInteger("oniStateKindId", (int)StateKind.Fighting);
            }
            animator.SetBool("isNoiseListener", false);
        }

        // 各パラメータを状態によって切り替える
        animator.SetFloat("moveSpeed", parameters[currentStateId].moveSpeed);
        animator.SetInteger("searchingTime", parameters[currentStateId].searchingTime);
        animator.SetFloat("searchingRange", parameters[currentStateId].searchingRange);
        animator.SetInteger("rayIrradiationTimeToFighting", parameters[currentStateId].rayIrradiationTimeToFighting);
        animator.SetInteger("rayBlockingTimeToVigilance", parameters[currentStateId].rayBlockingTimeToVigilance);
    }
}
