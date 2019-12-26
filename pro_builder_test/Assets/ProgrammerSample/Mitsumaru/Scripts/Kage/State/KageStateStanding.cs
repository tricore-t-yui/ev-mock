using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ParameterType = KageAnimParameterList.ParameterType;

/// <summary>
/// 影人間のステート：通常状態 / 待機型
/// </summary>
public class KageStateStanding : StateMachineBehaviour
{
    /// <summary>
    /// モーションの種類
    /// </summary>
    public enum MotionKind
    {
        StandBoltUpright = 1,    // 棒立ち
        LeanAgainstTheWall,      // 壁に寄りかかる
        HitAgainstTheWall,       // 壁に打ち付ける
        SleepSprawledOut,        // 大の字になる
    }

    // 現在のモーション
    MotionKind currentMotion = default;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // 待機位置
    Vector3 standingPosition = Vector3.zero;
    
    // 初期位置を保存したかどうか
    bool isSaveToStandingPos = false;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 前回のステートとしてセット
        animator.SetInteger("prevStateKindId", animator.GetInteger("currentStateKindId"));
        // 現在のステートのIDをセット
        animator.SetInteger("currentStateKindId", 0);

        // 待機位置を保存
        if (!isSaveToStandingPos)
        {
            standingPosition = animator.transform.position;
            isSaveToStandingPos = true;
        }

        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        // モーションの個数分の値からランダムで決定
        int motionId = Random.Range(1, System.Enum.GetNames(typeof(MotionKind)).Length + 1);
        // 決定した値をもとにモーションを変更する
        animParameterList.SetInteger(ParameterType.standingMotionKindId, motionId);

        // 影人間のメッシュレンダラーを取得
        MeshRenderer[] kageMeshRenderers = animator.GetComponentsInChildren<MeshRenderer>();

        // 影人間の全マテリアルを黒にする
        foreach (MeshRenderer meshRenderer in kageMeshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                material.color = Color.black;
            }
        }
    }

    /// <summary>
    /// 最初の待機位置に戻る
    /// </summary>
    public void ReturnStandingPosition()
    {
        // 最初の待機位置を移動目標位置に設定する
        navMesh.SetDestination(standingPosition);
    }
}
