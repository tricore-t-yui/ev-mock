using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 影人間のステート：追跡
/// </summary>
public class KageStateChaser : MonoBehaviour
{
    [SerializeField]
    EnemyVisibility enemyVisibility = default;

    [SerializeField]
    EnemyParameterIdList enemyParameterIdList = default;

    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    // プレイヤー
    [SerializeField]
    Transform player = default;

    [SerializeField]
    ColliderEvent attackRangeColliderEvent = default;

    // 移動スピード
    [SerializeField]
    float moveSpeed = default;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public void StateEnter()
    {
        // デリゲートをセットする
        enemyVisibility.SetOnLoseMomentDelegate(OnPlayerLoseMoment);
        attackRangeColliderEvent.AddEnterListener(OnAttack);
        // 移動スピードをセット
        navMeshAgent.speed = moveSpeed;
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public void StateUpdate()
    {
        // 追跡対象の位置をセット
        navMeshAgent.SetDestination(player.position);
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public void StateExit()
    {

    }

    /// <summary>
    /// プレイヤーを見失った時のコールバック
    /// </summary>
    void OnPlayerLoseMoment()
    {
        enemyParameterIdList.SetBool(EnemyParameterIdList.ParameterType.IsPlayerDiscover, false);
        enemyParameterIdList.SetBool(EnemyParameterIdList.ParameterType.IsPlayerSearching, true);
    }

    /// <summary>
    /// プレイヤーに攻撃した時
    /// </summary>
    void OnAttack(Transform self, Collider other)
    {
        // プレイヤーに当たったとき
        if (other.transform.GetHashCode() == player.transform.GetHashCode())
        {
            enemyParameterIdList.SetTrigger(EnemyParameterIdList.ParameterType.AttackStart);
        }
    }
}
