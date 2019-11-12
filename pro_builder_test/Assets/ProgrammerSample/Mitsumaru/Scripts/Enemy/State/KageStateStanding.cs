using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // パラメータクラスを取得
        animParameterList = animParameterList ?? animator.GetComponent<KageAnimParameterList>();

        // モーションの個数分の値からランダムで決定
        int motionId = Random.Range(1, System.Enum.GetNames(typeof(MotionKind)).Length + 1);
        // 決定した値をもとにモーションを変更する
        animParameterList.SetInteger(ParameterType.standingMotionKindId, motionId);
    }
}
