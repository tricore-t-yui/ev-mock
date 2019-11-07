using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間のサブステート：待機
/// </summary>
public class KageSubStateStanding : MonoBehaviour
{
    /// <summary>
    /// 待機モーション
    /// </summary>
    enum StandingMotion
    {
        StandBoltUpright,    // 棒立ち
        LeanAgainstTheWall,  // 壁に寄りかかる
        HitAgainstTheWall,   // 壁に打ち付ける
        SleepSprawledOut,    // 大の字になる
    }

    /// <summary>
    /// ステートの開始
    /// </summary>
    /// <param name="animator">アニメーター</param>
    public void StateEnter(Animator animator)
    {
        // 待機モーションの抽選を行う
        StandingMotion motion = LotteryMotion();
        // 決定したモーションに切り替える
        animator.SetInteger("StandingMotionPatternId", (int)motion);
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    /// <param name="animator">アニメーター</param>
    public void StateUpdate(Animator animator)
    {

    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    /// <param name="animator">アニメーター</param>
    public void StateExit(Animator animator)
    {

    }

    /// <summary>
    /// モーションの抽選を行う
    /// </summary>
    /// <returns></returns>
    StandingMotion LotteryMotion()
    {
        // 全モーションのIDをランダムで取得
        int motionId = Random.Range(0,System.Enum.GetNames(typeof(StandingMotion)).Length - 1);
        // 取得したIDのモーションを返す
        return (StandingMotion)motionId;
    }
}
