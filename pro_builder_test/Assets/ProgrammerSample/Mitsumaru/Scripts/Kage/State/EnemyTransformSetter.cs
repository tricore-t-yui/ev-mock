using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ハイドステートコントローラーを取得
        hideStateController = FindObjectOfType<HideStateController>() ?? hideStateController;
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // プレイヤーのクラスに敵のトランスフォームを渡す
        hideStateController.VisibleEnemy(animator.transform, enemyHeight);
    }
}
