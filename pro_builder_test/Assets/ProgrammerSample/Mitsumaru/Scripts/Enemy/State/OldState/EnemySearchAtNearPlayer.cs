using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// プレイヤー付近に向かいながら捜索を行う
/// </summary>
public class EnemySearchAtNearPlayer : StateMachineBehaviour
{
    // 目標位置の一番近くの距離範囲
    [SerializeField]
    float targetPosNearDistance = default;

    // 目標位置の最奥の距離範囲
    [SerializeField]
    float targetPosFarDistance = default;

    // プレイヤー
    [SerializeField]
    Transform player = default;

    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    [SerializeField]
    EnemyParameterIdList enemyParameterIdList = default;

    /// <summary>
    /// 開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 一番近い距離と最奥の距離の範囲で移動目標位置をランダムで決定する
        Vector3 targetPos = new Vector3(Random.Range(player.transform.position.x + targetPosNearDistance, player.transform.position.x + targetPosFarDistance),
                                        Random.Range(player.transform.position.y + targetPosNearDistance, player.transform.position.y + targetPosFarDistance),
                                        Random.Range(player.transform.position.z + targetPosNearDistance, player.transform.position.z + targetPosFarDistance));

        // 目標位置をセット
        navMeshAgent.SetDestination(targetPos);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animatorStateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 目標位置まできたら徘徊に戻る
        if (navMeshAgent.remainingDistance < 0.5f)
        {
            enemyParameterIdList.SetBool(EnemyParameterIdList.ParameterType.IsPlayerSearching, false);
        }
    }
}
