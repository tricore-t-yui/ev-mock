using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間のステートマシン
/// </summary>
public class KageMainStateMachine : MonoBehaviour
{
    /// <summary>
    /// 影人間のステート
    /// </summary>
    public enum State
    {
        Normal,    // 通常状態
        Vigilant,  // 警戒状態
        Attack,    // 攻撃状態
    }

    // 影人間のアニメーター
    [SerializeField]
    Animator kageAnimator = default;

    // 影人間のステートマシン
    StateMachine<State> kageMainStateMachine;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // ステートマシンのインスタンスを生成（アニメーターを渡す
        kageMainStateMachine = new StateMachine<State>(kageAnimator);

        // 各ステートを追加
        // TODO : 各ステートの関数を引数に追加する
        kageMainStateMachine.Add(State.Normal);
        kageMainStateMachine.Add(State.Vigilant);
        kageMainStateMachine.Add(State.Attack);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // ステートマシンの更新処理
        kageMainStateMachine.Update();
    }
}
