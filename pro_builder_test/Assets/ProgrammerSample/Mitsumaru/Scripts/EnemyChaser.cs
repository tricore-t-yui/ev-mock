using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// プレイヤーの追跡を行うエネミー
/// </summary>
public class EnemyChaser : MonoBehaviour
{
    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    // プレイヤー
    [SerializeField]
    Transform player = default;

    // 移動スピード
    [SerializeField]
    float moveSpeed = default;

    /// <summary>
    /// 開始
    /// </summary>
    void OnEnable()
    {
        // 移動スピードをセット
        navMeshAgent.speed = moveSpeed;
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 追跡対象の位置をセット
        navMeshAgent.SetDestination(player.position);
    }
}
