using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵のトランスフォームを渡す
/// </summary>
public class EnemyTransformSetter : StateMachineBehaviour
{
    // ハイドステートコントローラー
    HideStateController hideStateController = null;

    // 敵の高さ
    [SerializeField]
    float enemyHeight = 0;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // 敵のサウンドプレイヤー取得
    EnemySoundPlayer soundPlayer = null;

    float stepSoundInterval = 0;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュのコンポーネントを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // ハイドステートコントローラーを取得
        hideStateController = FindObjectOfType<HideStateController>() ?? hideStateController;
        // サウンドプレイヤー
        soundPlayer = animator.GetComponentInChildren<EnemySoundPlayer>() ?? soundPlayer;

    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // プレイヤーのクラスに敵のトランスフォームを渡す
        hideStateController.VisibleEnemy(animator.transform, enemyHeight);

        stepSoundInterval -= navMesh.velocity.magnitude;
        if (stepSoundInterval < 0)
        {
            soundPlayer.Play("Step");
            stepSoundInterval = 40;
        }
    }
}
