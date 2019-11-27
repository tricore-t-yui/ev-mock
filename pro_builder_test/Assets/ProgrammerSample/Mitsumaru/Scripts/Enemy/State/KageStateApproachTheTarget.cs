using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ParameterType = KageAnimParameterList.ParameterType;

/// <summary>
/// 対象へ接近する
/// </summary>
public class KageStateApproachTheTarget : StateMachineBehaviour
{
    // 移動速度
    [SerializeField]
    float speed = 0;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // 目標地点での停止時間
    [SerializeField]
    int targetPosStopTime = 0;
    // カウンター
    int stopTimeCounter = 0;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュのコンポーネントを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // 移動速度をセット
        navMesh.speed = speed;

        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        // 接近対象の位置を取得
        Vector3 targetPos = Vector3.zero;
        targetPos.x = animParameterList.GetFloat(ParameterType.targetPositionX);
        targetPos.y = animParameterList.GetFloat(ParameterType.targetPositionY);
        targetPos.z = animParameterList.GetFloat(ParameterType.targetPositionZ);

        // 目標位置をセット
        navMesh.SetDestination(targetPos);
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 目標位置に到着
        if (navMesh.remainingDistance < 0.3f)
        {
            // その場で待機する
            animParameterList.SetTrigger(ParameterType.targetPosStop);
        }
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 一度オンにしたトリガーをリセットする
        animParameterList.ResetTrigger(ParameterType.targetPosStop);
    }
}
