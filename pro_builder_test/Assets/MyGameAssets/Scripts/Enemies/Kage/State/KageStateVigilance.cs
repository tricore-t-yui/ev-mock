using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間のステート：警戒状態
/// </summary>
public class KageStateVigilance : StateMachineBehaviour
{
    // 視野の範囲
    KageFieldOfView fieldOfView = null;
    // 警戒範囲
    KageVigilanceRange vigilanceRange = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 視野の範囲
        fieldOfView = animator.transform.Find("Collider").Find("KageFeildOfView").GetComponent<KageFieldOfView>() ?? fieldOfView;
        // 視野の範囲を設定する
        fieldOfView.ChangeDistance(KageState.Kind.Vigilance);

        // 警戒範囲
        vigilanceRange = animator.transform.Find("Collider").Find("KageVigilanceRange").GetComponent<KageVigilanceRange>() ?? vigilanceRange;
        //警戒範囲の設定を行う
        vigilanceRange.ChangeRadius(KageState.Kind.Vigilance);
    }
}
