/******************************************************************************/
/*!    \brief  ステートマシン.
*******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public StateMachine(Animator animator)
    {
       this.animator = animator;
    }

    Animator animator = default;

    /// <summary>
    /// ステート.
    /// </summary>
    public class State
    {
        Action<Animator> EnterAct;  // 開始時に呼び出されるデリゲート.
        Action<Animator> UpdateAct; // 更新時に呼び出されるデリゲート.
        Action<Animator> ExitAct;   // 終了時に呼び出されるデリゲート.

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        public State(Action<Animator> enterAct = null, Action<Animator> updateAct = null, Action<Animator> exitAct = null)
        {
            EnterAct = enterAct ?? delegate { };
            UpdateAct = updateAct ?? delegate { };
            ExitAct = exitAct ?? delegate { };
        }

        /// <summary>
        /// 開始します.
        /// </summary>
        public void Enter(Animator animator)
        {
            EnterAct(animator);
        }

        /// <summary>
        /// 更新します.
        /// </summary>
        public void Update(Animator animator)
        {
            UpdateAct(animator);
        }

        /// <summary>
        /// 終了します.
        /// </summary>
        public void Exit(Animator animator)
        {
            ExitAct(animator);
        }
    }

    Dictionary<T, State> StateTable = new Dictionary<T, State>();   // ステートのテーブル.
    State CurrentState;                                             // 現在のステート.
    T CurrentStateKey;                                              // 現在のステートキー.

    /// <summary>
    /// ステートを追加します.
    /// </summary>
    public void Add(T key, Action<Animator> enterAct = null, Action<Animator> updateAct = null, Action<Animator> exitAct = null)
    {
        StateTable.Add(key, new State(enterAct, updateAct, exitAct));
    }

    /// <summary>
    /// 現在のステートを設定します.
    /// </summary>
    public void SetState(T key)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit(animator);
        }
        CurrentStateKey = key;
        CurrentState = StateTable[key];
        CurrentState.Enter(animator);
    }

    /// <summary>
    /// 現在のステートを取得します.
    /// </summary>
    public T GetState()
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
        CurrentState.Update(animator);
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