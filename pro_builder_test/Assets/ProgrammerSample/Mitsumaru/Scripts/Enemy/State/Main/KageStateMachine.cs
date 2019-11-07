using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 影人間のステートマシン
/// </summary>
public class KageStateMachine : MonoBehaviour
{
    // 影人間のアニメーター
    [SerializeField]
    Animator kageAnimator = default;
    // 影人間のステートマシン
    StateMachine kageMainStateMachine;

    int currentStateHash = 0;
    int prevStateHash = 0;

    [SerializeField] KageStateWandering          stateWandering          = default;
    [SerializeField] KageStateChaser             stateChaser             = default;
    [SerializeField] KageStateSearchAtCheckPoint stateSearchAtCheckPoint = default;
    [SerializeField] KageStateSearchAtNearPlayer stateSearchAtNearPlayer = default;
    [SerializeField] KageStateSurvey             stateSurvey             = default;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // ステートマシンのインスタンスを生成（アニメーターを渡す
        kageMainStateMachine = new StateMachine();

        // 各ステートを追加
        // TODO : 各ステートの関数を引数に追加する
        kageMainStateMachine.Add("Wandering", stateWandering.StateEnter,stateWandering.StateUpdate,stateWandering.StateExit);
        kageMainStateMachine.Add("Chaser", stateChaser.StateEnter, stateChaser.StateUpdate, stateChaser.StateExit);
        kageMainStateMachine.Add("SearchAtCheckPoint", stateSearchAtCheckPoint.StateEnter, stateSearchAtCheckPoint.StateUpdate, stateSearchAtCheckPoint.StateExit);
        kageMainStateMachine.Add("SearchAtNearPlayer", stateSearchAtNearPlayer.StateEnter, stateSearchAtNearPlayer.StateUpdate, stateSearchAtNearPlayer.StateExit);
        kageMainStateMachine.Add("Survey", stateSurvey.StateEnter, stateSurvey.StateUpdate, stateSurvey.StateExit);
        kageMainStateMachine.Add("Attack");

        // 最初のステートを「通常状態」に設定
        int hash = Animator.StringToHash("Base Layer.Wandering");
        kageMainStateMachine.SetState(hash);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // ステートマシンの更新処理
        kageMainStateMachine.Update();
        // ステートの変更
        ChangeState();
    }

    /// <summary>
    /// ステートの変更
    /// </summary>
    void ChangeState()
    {
        // 前フレームのステートのハッシュ
        prevStateHash = currentStateHash;
        // 現在のステートのハッシュ
        currentStateHash = kageAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        // ステートが変更されたら
        if (prevStateHash != currentStateHash)
        {
            // ステートマシンのステートを変更する
            kageMainStateMachine.SetState(currentStateHash);
        }
    }
}
