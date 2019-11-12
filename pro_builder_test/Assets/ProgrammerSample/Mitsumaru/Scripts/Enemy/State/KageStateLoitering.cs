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
    [SerializeField]
    LoiteringKind loiteringKind = LoiteringKind.Route;

    // 移動タイプ：ルート
    [SerializeField]
    KageStateMoveAtRoute loiteringMoveAtRoute = default;

    // 現在の移動タイプ
    StateMachineBehaviour currentLoiteringMove;
    
    /// <summary>
    /// 起動処理
    /// </summary>
    void Awake()
    {
        // NOET : 現在はルートでの徘徊しか完成していないためそのままセット
        currentLoiteringMove = loiteringMoveAtRoute;
    }

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        currentLoiteringMove.OnStateEnter(animator, animatorStateInfo, layerIndex);
    }
    
    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        currentLoiteringMove.OnStateUpdate(animator, animatorStateInfo, layerIndex);
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        currentLoiteringMove.OnStateExit(animator, animatorStateInfo, layerIndex);
    }
}
