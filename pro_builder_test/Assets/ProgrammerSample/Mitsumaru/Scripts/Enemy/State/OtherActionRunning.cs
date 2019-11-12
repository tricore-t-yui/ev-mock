using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OtherActionRunning : StateMachineBehaviour
{
    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // 走るスピード
    [SerializeField]
    float runSpeed = 1;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュのコンポーネントを取得
        navMesh = navMesh ?? animator.GetComponent<NavMeshAgent>();

        // 走るスピードを設定
        navMesh.speed = runSpeed;
    }
}
