using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KageLoiteringOtherAction : StateMachineBehaviour
{
    [System.Serializable]
    class DebugSettings
    {
        public bool isDebug = false;
        public int playActionId = 1;
    }

    // その他の行動の数
    [SerializeField]
    int actionNum = 0;

    // デバッグ用設定クラス
    [SerializeField]
    DebugSettings debugSettings = default;


    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // デバッグ用の処理
        if (debugSettings.isDebug)
        {
            // 決定した行動に設定
            animator.SetInteger("OtherActionKindId", debugSettings.playActionId);
        }
        else
        {
            // 徘徊中の行動の抽選割合を取得
            float loiteringBehaviorRate = animator.GetInteger("LoiteringBehaviorRate");
            // 徘徊中のアクションの等倍抽選を行う
            float otherActionRate = Random.Range(0, 100 - loiteringBehaviorRate + 1);
            // 決定した値からアクションのIDを算出
            int actionId = (int)(otherActionRate / ((100 - loiteringBehaviorRate) / actionNum));
            // 算出したIDが範囲外であれば合わせる
            if (actionId >= 0) { actionId++; }
            if (actionId >  8) { actionId = actionNum; }

            // 決定した行動に設定
            animator.SetInteger("OtherActionKindId", actionId);
        }
    }
}
