using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParameterType = KageAnimParameterList.ParameterType;

/// <summary>
/// 影人間の徘徊中の「その他の行動」の抽選を行う
/// </summary>
public class KageLoiteringOtherAction : StateMachineBehaviour
{
    /// <summary>
    /// デバッグ用の設定クラス
    /// </summary>
    [System.Serializable]
    class DebugSettings
    {
        public bool isDebug = false;
        public int playActionId = 1;
        public bool isOrderBehavior = false;
    }

    // その他の行動の数
    [SerializeField]
    int actionNum = 0;

    // デバッグ用設定クラス
    [SerializeField]
    DebugSettings debugSettings = default;

    // 現在のアクション番号
    int currentAction = 0;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;


    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        if (debugSettings.isOrderBehavior)
        {
            // 次の行動派生を再生するか
            if (animator.GetBool("isPlayingNextBehavior"))
            {
                // 行動派生の番号を次へ
                currentAction++;

                if (currentAction > actionNum)
                {
                    currentAction = 0;
                }

                // 再生フラグをたおす
                animator.SetBool("isPlayingNextBehavior", false);
            }

            // 決定した行動に設定
            animParameterList.SetInteger(ParameterType.otherActionKindId, currentAction);
        }
        else
        {
            // デバッグ用の処理
            if (debugSettings.isDebug)
            {
                // 決定した行動に設定
                animParameterList.SetInteger(ParameterType.otherActionKindId, debugSettings.playActionId);
            }
            // 通常処理
            else
            {
                // 徘徊中の行動の抽選割合を取得
                float loiteringBehaviorRate = animParameterList.GetFloat(ParameterType.loiteringBehaviorRate);
                // 徘徊中のアクションの等倍抽選を行う
                float otherActionRate = Random.Range(0, 100 - loiteringBehaviorRate + 1);
                // 決定した値からアクションのIDを算出
                int actionId = (int)(otherActionRate / ((100 - loiteringBehaviorRate) / actionNum));
                // 算出したIDが範囲外であれば合わせる
                if (actionId >= 0) { actionId++; }
                if (actionId > 8) { actionId = actionNum; }

                // 決定した行動に設定
                animParameterList.SetInteger(ParameterType.otherActionKindId, actionId);
            }
        }
    }
}
