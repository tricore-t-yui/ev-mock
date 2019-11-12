using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間のステート：通常状態 / 徘徊型
/// NOTE : ルートかランダムかで動作させるスクリプトを変更する
/// </summary>
public class KageStateLoitering : StateMachineBehaviour
{
    /// <summary>
    /// 徘徊の種類
    /// </summary>
    public enum LoiteringKind
    {
        Route = 1,      // ルート
        Random,         // ランダム
    }

    // 徘徊の種類
    LoiteringKind loiteringType = LoiteringKind.Route;

    // 徘徊タイプ：ルート
    [SerializeField]
    KageStateMoveAtRoute loiteringMoveAtRoute = default;
    // 徘徊タイプ：ランダム
    [SerializeField]
    KageStateMoveAtRandom loiteringMoveAtRandom = default;

    // 現在の移動タイプ
    StateMachineBehaviour currentLoiteringMove;

    // 影人間のステートのパラーメータを取得
    KageStateParameter stateParameter = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ステートパラメータを取得
        stateParameter = animator.GetComponent<KageStateParameter>() ?? stateParameter;

        // 徘徊のタイプによってクラスを分ける
        if (stateParameter.StateLoiteringOfType == LoiteringKind.Route)
        {
            currentLoiteringMove = loiteringMoveAtRoute;
        }
        else
        {
            currentLoiteringMove = loiteringMoveAtRandom;
        }

        // ステートの開始
        currentLoiteringMove.OnStateEnter(animator, animatorStateInfo, layerIndex);
    }
    
    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ステートの更新
        currentLoiteringMove.OnStateUpdate(animator, animatorStateInfo, layerIndex);
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ステートの終了
        currentLoiteringMove.OnStateExit(animator, animatorStateInfo, layerIndex);
    }
}
