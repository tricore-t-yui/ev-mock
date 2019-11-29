using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Linq;
using ParameterType = KageAnimParameterList.ParameterType;

/// <summary>
/// 影人間のステート：通常状態 / 徘徊型 / ルート移動
/// </summary>
public class KageStateMoveAtRoute : VigilanceMoveBase
{
    // 移動チェックポイント
    IReadOnlyList<Vector3> checkPointList = default;

    // 現在のチェックポイントのインデックス
    int currentCheckPointIndex = 0;

    // 移動中のカウント
    int moveCount = 0;

    // 移動スピード
    [SerializeField]
    float moveSpeed = 1;

    // 移動の間隔
    [Space(10)]
    [SerializeField]
    int moveInterval = 0;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // 影人間のステートのパラーメータを取得
    KageStateParameter stateParameter = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ステートパラメータを取得
        stateParameter = animator.GetComponent<KageStateParameter>() ?? stateParameter;
        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;
        // チェックポイントを取得
        checkPointList = stateParameter.RouteCheckPointList;

        // ナビメッシュの取得
        GetNavMeshAgent(animator);

        // 移動スピードを設定
        navMesh.speed = moveSpeed;

        // 最初のチェックポイントを設定
        currentCheckPointIndex = 0;
        navMesh.SetDestination(checkPointList[currentCheckPointIndex]);

        // 移動を開始する
        animParameterList.SetBool(ParameterType.isLoiteringMove, true);
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 目標のチェックポイントに着いたら
        if (navMesh.remainingDistance < 0.3f)
        {
            // 次のチェックポイントを設定する
            currentCheckPointIndex = GetNextCheckPointIndex();
            navMesh.SetDestination(checkPointList[currentCheckPointIndex]);
        }
        // 移動中のカウンター
        moveCount++;

        // 一定時間移動し続けたら
        if (moveCount >= moveInterval)
        {
            // 移動を中断する
            animParameterList.SetBool(ParameterType.isLoiteringMove, false);
            // カウンターをリセット
            moveCount = 0;
        }
    }

    /// <summary>
    /// 元の徘徊地点に戻る
    /// </summary>
    public override void ReturnVigilancePoint(Animator animator)
    {
        // 一番近いチェックポイントを取得する
        Vector3 returnPos = checkPointList.OrderByDescending(elem => (animator.transform.position-elem).magnitude * -1).FirstOrDefault();
        currentCheckPointIndex = checkPointList.ToList().IndexOf(returnPos);
        // 次の目標位置にセット
        navMesh.SetDestination(returnPos);
    }

    /// <summary>
    /// 次のチェックポイントのインデックスを取得
    /// </summary>
    /// <returns></returns>
    int GetNextCheckPointIndex()
    {
        if (currentCheckPointIndex > checkPointList.Count -1)
        {
            return 0;
        }
        else
        {
            return currentCheckPointIndex+1;
        }
    }
}
