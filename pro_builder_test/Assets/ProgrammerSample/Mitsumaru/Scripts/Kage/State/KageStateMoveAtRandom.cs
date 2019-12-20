using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ParameterType = KageAnimParameterList.ParameterType;

/// <summary>
/// 影人間のステート：通常状態 / 徘徊型 / ランダム移動
/// </summary>
public class KageStateMoveAtRandom : StateMachineBehaviour
{
    // 範囲用コライダー
    RandomMoveRangeCollider rangeCollider = null;

    // 移動中のカウント
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
        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;
        // ステートパラメータを取得
        stateParameter = animator.GetComponent<KageStateParameter>() ?? stateParameter;

        // 範囲用コライダーのオブジェクトを取得
        GameObject colliderObject = GameObject.Find(stateParameter.RandomMoveRangeOfObjectName);
        // オブジェクトからコライダーのスクリプトを取得
        rangeCollider = colliderObject.GetComponent<RandomMoveRangeCollider>() ?? rangeCollider;

        // 範囲から抜けた瞬間に呼ばれる関数をセット
        rangeCollider.SetCallback(OnRangeExit);

        // ナビメッシュの取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // 移動スピードを設定
        navMesh.speed = moveSpeed;

        // 移動を開始する
        animParameterList.SetBool(ParameterType.isLoiteringMove, true);

        // 新たに目標位置を設定
        navMesh.SetDestination(GetNextTargetPos());
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 目標位置に到着したら
        if (navMesh.remainingDistance < navMesh.stoppingDistance)
        {
            // 新たに目標位置を設定
            navMesh.SetDestination(GetNextTargetPos());
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
    /// 次の目標位置を取得
    /// </summary>
    /// <returns></returns>
    Vector3 GetNextTargetPos()
    {
        // コライダーのトランスフォームを取得
        Transform colliderTrans =  rangeCollider.transform;
        // コライダーの半径を取得
        float colliderRadius = rangeCollider.GetRadius();
        // コライダーの範囲内で目標位置を取得
        Vector3 nextPos = new Vector3(Random.Range(colliderTrans.position.x - colliderRadius, colliderTrans.position.x + colliderRadius),
                                      colliderTrans.position.y,
                                      Random.Range(colliderTrans.position.z - colliderRadius, colliderTrans.position.z + colliderRadius));

        return nextPos;
    }

    /// <summary>
    ///  範囲から抜けた瞬間
    /// </summary>
    void OnRangeExit(string enemyTag)
    {
        // 範囲外に出ようとしているオブジェクトが影人間だったら
        if (enemyTag == "Kage")
        {
            // 新たに目標位置を設定
            navMesh.SetDestination(GetNextTargetPos());
        }
    }

    /// <summary>
    /// 元の徘徊位置に戻る
    /// </summary>
    public void ReturnLoiteringPoint(Animator animator)
    {

    }
}
