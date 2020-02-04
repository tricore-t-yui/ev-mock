using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ハイドポイント付近を移動
/// </summary>
public class OniStateNearTheHideMove : StateMachineBehaviour
{
    // 移動時間のカウンター
    int moveTimeCounter = 0;

    // その他の行動の遷移インターバル
    [SerializeField]
    int otherActionTime = 0;
    int otherActionTimeCounter = 0;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // 移動範囲の中心位置
    Vector3 center = Vector3.zero;

    // 基準位置をセットしたかどうか
    bool isSetCenterPos = false;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // 移動速度をセット
        navMesh.speed = animator.GetFloat("moveSpeed");

        if (!isSetCenterPos)
        {
            // 到着した位置を基準位置に設定
            center = animator.transform.position;

            // 移動範囲の中心位置をセットしたフラグをオンにする
            isSetCenterPos = true;
        }

        // 移動範囲をパラメータから取得
        float searchingRange = animator.GetFloat("searchingRange");
        // ランダムで位置をセット
        Vector3 targetPos = new Vector3(Random.Range(center.x - searchingRange, center.x + searchingRange),
                                        Random.Range(center.y - searchingRange, center.y + searchingRange),
                                        Random.Range(center.z - searchingRange, center.z + searchingRange));

        // 目標位置をセット
        navMesh.SetDestination(targetPos);
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 目標位置に着いたら、目標位置を更新して移動を続行
        if (navMesh.remainingDistance < navMesh.stoppingDistance)
        {
            animator.SetTrigger("moveEndContinue");
        }


        otherActionTimeCounter++;
        // その他の行動開始時間になったら、抽選を開始する
        if (otherActionTimeCounter >= otherActionTime)
        {
            animator.SetTrigger("lotteryStart");
            otherActionTimeCounter = 0;
        }

        moveTimeCounter++;

        // 探索時間が過ぎたら終了する
        if (moveTimeCounter >= animator.GetInteger("searchingTime"))
        {
            moveTimeCounter = 0;
            animator.SetBool("isNearTheHideMove", false);
        }
    }
}
