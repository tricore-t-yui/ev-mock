using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 通常状態に復帰するステートの基底クラス
/// </summary>
public class ReturnNormalStateBase : StateMachineBehaviour
{
    // ナビメッシュ
    protected NavMeshAgent navMesh = null;

    /// <summary>
    /// ナビメッシュの取得
    /// </summary>
    protected void GetNavMeshAgent(Animator animator)
    {
        // ナビメッシュの取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
    }

    /// <summary>
    /// 元の徘徊地点に戻る
    /// </summary>
    public virtual void Return(Animator animator) { }
}
