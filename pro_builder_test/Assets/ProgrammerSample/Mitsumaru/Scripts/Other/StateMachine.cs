/******************************************************************************/
/*!    \brief  ステートマシン.
*******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    /// <summary>
    /// ステート.
    /// </summary>
    class State
    {
        readonly Action EnterAct;  // 開始時に呼び出されるデリゲート.
        readonly Action UpdateAct; // 更新時に呼び出されるデリゲート.
        readonly Action ExitAct;   // 終了時に呼び出されるデリゲート.

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        public State(Action enterAct = null, Action updateAct = null, Action exitAct = null)
        {
            EnterAct = enterAct ?? delegate { };
            UpdateAct = updateAct ?? delegate { };
            ExitAct = exitAct ?? delegate { };
        }

        /// <summary>
        /// 開始します.
        /// </summary>
        public void Enter()
        {
            EnterAct();
        }

        /// <summary>
        /// 更新します.
        /// </summary>
        public void Update()
        {
            UpdateAct();
        }

        /// <summary>
        /// 終了します.
        /// </summary>
        public void Exit()
        {
            ExitAct();
        }
    }

    Dictionary<int, State> StateTable = new Dictionary<int, State>();   // ステートのテーブル.
    State CurrentState;                                                 // 現在のステート.
    int CurrentStateKey;                                                // 現在のステートキー.

    /// <summary>
    /// ステートを追加します.
    /// </summary>
    public void Add(string key, Action enterAct = null, Action updateAct = null, Action exitAct = null)
    {
        int hash = Animator.StringToHash("Base Layer." + key);
        StateTable.Add(hash, new State(enterAct, updateAct, exitAct));
    }

    /// <summary>
    /// 現在のステートを設定します.
    /// </summary>
    public void SetState(int key)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }
        CurrentStateKey = key;
        CurrentState = StateTable[key];
        CurrentState.Enter();
    }

    /// <summary>
    /// 現在のステートを取得します.
    /// </summary>
    public int GetState()
    {
        return CurrentStateKey;
    }


    /// <summary>
    /// 現在のステートを更新します.
    /// </summary>
    public void Update()
    {
        if (CurrentState == null)
        {
            return;
        }
        CurrentState.Update();
    }

    /// <summary>
    /// すべてのステートを削除します.
    /// </summary>
    public void Clear()
    {
        StateTable.Clear();
        CurrentState = null;
    }
}