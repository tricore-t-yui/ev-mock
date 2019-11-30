using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 徘徊の基底クラス
/// </summary>
public abstract class VigilanceMoveBase : StateMachineBehaviour
{
    // ナビメッシュ
    protected NavMeshAgent navMesh = null;

    /// <summary>
    /// ナビメッシュの取得
    /// </summary>
    protected void GetNavMeshAgent(Animator animator)
    {
        if (navMesh == null)
        {
            // ナビメッシュの取得
            navMesh = animator.gameObject.GetComponent<NavMeshAgent>();
        }
        else
        {
           // Debug.Log("else");
        }
    }

    /// <summary>
    /// 元の徘徊地点に戻る
    /// </summary>
    public virtual void ReturnVigilancePoint(Animator animator) { }
}
