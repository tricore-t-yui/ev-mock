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

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // 影人間のステートのパラーメータを取得
    KageStateParameter stateParameter = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 前回のステートとしてセット
        animator.SetInteger("prevStateKindId", animator.GetInteger("currentStateKindId"));
        // 現在のステートのIDをセット
        animator.SetInteger("currentStateKindId", 0);

        // 状態レベルを変更
        animator.SetInteger("currentStateLevel", 0);

        // ステートパラメータを取得
        stateParameter = animator.GetComponent<KageStateParameter>() ?? stateParameter;
        // パラメータクラスを取得
        animParameterList = animator.gameObject.GetComponent<KageAnimParameterList>();

        // 徘徊のタイプによってクラスを分ける
        // ルートタイプ
        if (stateParameter.StateLoiteringOfType == LoiteringKind.Route)
        {
            animParameterList.SetInteger(KageAnimParameterList.ParameterType.loiteringKindId, 1);
        }
        // ランダムタイプ
        else
        {
            animParameterList.SetInteger(KageAnimParameterList.ParameterType.loiteringKindId, 2);
        }


        // 影人間のメッシュレンダラーを取得
        SkinnedMeshRenderer[] kageMeshRenderers = animator.GetComponentsInChildren<SkinnedMeshRenderer>();

        // 影人間の全マテリアルを黒にする
        foreach (SkinnedMeshRenderer meshRenderer in kageMeshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                material.color = Color.black;
            }
        }
    }
}
