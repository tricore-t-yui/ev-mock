using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowStateNormal : MonoBehaviour
{
    // アニメーター
    [SerializeField]
    Animator animator = default;
    [SerializeField]
    NavMeshAgent agent = default;

    [SerializeField]
    ShadowStateChanger shadowStateChanger = default;

    // 影人間のメッシュ
    [SerializeField]
    SkinnedMeshRenderer shadowBodyMesh = default;

    void OnEnable()
    {
        // 影人間の姿を消す
        shadowBodyMesh.enabled = false;
    }

    void OnDisable()
    {
        // 影人間が姿を現す
        shadowBodyMesh.enabled = true;
    }

    /// <summary>
    /// 警戒状態に入った
    /// </summary>
    public void OnCautionRangeEnter(Collider other)
    {
        // 通常状態中のみ
        if (shadowStateChanger.CurrentStateTypeId != (int)ShadowParameter.StateType.Normal) { return; }

        // 注意状態に変更
        animator.SetInteger("StateTypeId", (int)ShadowParameter.StateType.Alert);
        // 辺りに注意しているときの待機フラグをオンにする
        animator.SetBool("IsAlertWaiting", true);
    }

    /// <summary>
    /// 音を聞いた
    /// </summary>
    public void OnListenNoise(Collider other)
    {
        // 警戒状態中のみ
        if (shadowStateChanger.CurrentStateTypeId != (int)ShadowParameter.StateType.Caution) { return; }

        // 警戒に移行
        animator.SetInteger("StateTypeId", (int)ShadowParameter.StateType.Caution);
        // 移動目標位置を音の発信源に
        agent.SetDestination(other.transform.position);
    }

    /// <summary>
    /// 対象を見つけた
    /// </summary>
    /// <param name="other"></param>
    public void OnShadowVisible(Collider other)
    {
        // 戦闘状態に変更
        animator.SetInteger("StateTypeId", (int)ShadowParameter.StateType.Fighting);
        // 対象の位置を目標位置にする
        agent.SetDestination(other.transform.position);
    }
}
