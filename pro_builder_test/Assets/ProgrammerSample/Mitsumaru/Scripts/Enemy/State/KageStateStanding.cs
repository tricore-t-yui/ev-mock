using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // モーションの個数分の値からランダムで決定
        int motionId = Random.Range(1, System.Enum.GetNames(typeof(MotionKind)).Length + 1);
        // 決定した値をもとにモーションを変更する
        animator.SetInteger("StandingMotionKindId", motionId);
    }
}
