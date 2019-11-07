using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 影人間のステート：徘徊
/// </summary>
public class KageStateWandering : MonoBehaviour
{
    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    // 影人間の視野
    [SerializeField]
    EnemyVisibility enemyVisibility = default;

    // 敵のコライダーイベント
    [SerializeField]
    ColliderEvent enemyHearColliderEvent = default;

    [SerializeField]
    EnemyParameterIdList enemyParameterIdList = default;

    // 移動スピード
    [SerializeField]
    float moveSpeed = default;

    // 目標位置のリスト
    [SerializeField]
    List<Vector3> targetPositions = default;

    // 現在の目標位置のリスト番号
    int currentIndex = 0;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public void StateEnter()
    {
        // デリゲートをセットする
        enemyVisibility.SetOnDiscoverMomentDelegate(OnPlayerDiscoverMoment);

        // 移動スピードをセット
        navMeshAgent.speed = moveSpeed;
        // 一番最初の位置を設定
        navMeshAgent.SetDestination(GetNextTargetPos());
        // コライダーイベントを追加
        enemyHearColliderEvent.AddEnterListener(OnListenPlayerNise);
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public void StateUpdate()
    {
        // 移動が完了したら次の目標位置を設定する
        if (navMeshAgent.remainingDistance < 0.5f)
        {
            // 次の目標位置を取得
            Vector3 nextTargetPos = GetNextTargetPos();
            // 目標位置をセット
            navMeshAgent.SetDestination(nextTargetPos);
        }
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public void StateExit()
    {

    }

    /// <summary>
    /// 次の目標位置を取得
    /// </summary>
    /// <returns>次の目標位置</returns>
    Vector3 GetNextTargetPos()
    {
        // リストの末尾まできていたら先頭に戻る
        if (currentIndex == targetPositions.Count - 1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

        // 次の目標位置を返す
        return targetPositions[currentIndex];
    }

    /// <summary>
    /// プレイヤーの物音を聴いたとき
    /// </summary>
    /// note : 引数はUnityEventのエラー回避のため使わない。
    void OnListenPlayerNise(Transform self, Collider other)
    {
        // 捜索フラグをオンにする
        enemyParameterIdList.SetBool(EnemyParameterIdList.ParameterType.IsPlayerSearching, true);
    }

    /// <summary>
    /// プレイヤーを発見した時のコールバック
    /// </summary>
    void OnPlayerDiscoverMoment()
    {
        enemyParameterIdList.SetBool(EnemyParameterIdList.ParameterType.IsPlayerDiscover, true);
    }
}
