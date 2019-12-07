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
public class KageStateMoveAtRoute : StateMachineBehaviour
{
    // 移動チェックポイント
    IReadOnlyList<Vector3> checkPointList = default;

    // 現在のチェックポイントのインデックス
    [System.NonSerialized]
    int currentCheckPointIndex = 0;

    // 移動中のカウント
    [System.NonSerialized]
    int moveCount = 0;

    // 移動スピード
    [SerializeField]
    float moveSpeed = 1;

    // 移動の間隔
    [Space(10)]
    [SerializeField]
    int moveInterval = 0;

    NavMeshAgent navMesh = null;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // 影人間のステートのパラーメータを取得
    KageStateParameter stateParameter = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Debug.Log(currentCheckPointIndex);

        // ステートパラメータを取得
        stateParameter = animator.gameObject.GetComponent<KageStateParameter>();
        // パラメータクラスを取得
        animParameterList = animator.gameObject.GetComponent<KageAnimParameterList>();
        // チェックポイントを取得
        checkPointList = stateParameter.RouteCheckPointList;

        // ナビメッシュの取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // 移動スピードを設定
        navMesh.speed = moveSpeed;

        navMesh.isStopped = false;
        
        // 最初のチェックポイントを設定
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
        if (navMesh.remainingDistance < navMesh.stoppingDistance)
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
    public void ReturnVigilancePoint(Animator animator)
    {
        // 一番近いチェックポイントを取得する
        //Vector3 returnPos = checkPointList.OrderByDescending(elem => (animator.transform.position-elem).magnitude * -1).FirstOrDefault();
        //currentCheckPointIndex = checkPointList.ToList().IndexOf(returnPos);
        // 次の目標位置にセット
        navMesh.SetDestination(checkPointList[currentCheckPointIndex]);
    }

    /// <summary>
    /// 次のチェックポイントのインデックスを取得
    /// </summary>
    /// <returns></returns>
    int GetNextCheckPointIndex()
    {
        if (currentCheckPointIndex >= checkPointList.Count -1)
        {
            return 0;
        }
        else
        {
            return currentCheckPointIndex+1;
        }
    }
}
