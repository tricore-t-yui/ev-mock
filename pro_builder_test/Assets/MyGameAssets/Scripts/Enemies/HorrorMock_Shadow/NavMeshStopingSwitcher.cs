using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshStopingSwitcher
{
    // 移動の状態
    public enum AnmationTag
    {
        WaitStart,   // 待機開始
        WaitLoop,    // 待機中
        WaitEnd,     // 待機終了
        MoveStart,   // 開始
        MoveLoop,    // 移動中
        MoveEnd,     // 終了
        Attack,      // 攻撃
    }

    Animator animator = default;
    NavMeshAgent agent = default;

    // アニメーターのタグのハッシュ
    int currentAnimatorTagHash = 0;
    int prevAnimatorTagHash = 0;

    // タグハッシュテーブル
    Dictionary<AnmationTag, int> tagHashTable = new Dictionary<AnmationTag, int>();

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="agent"></param>
    public void Initialize(Animator animator,NavMeshAgent agent)
    {
        this.animator = animator;
        this.agent = agent;

        // ステートごとの名前を文字列で取得
        string[] tagNames = System.Enum.GetNames(typeof(AnmationTag));
        foreach(string tagName in tagNames)
        {
            AnmationTag tag;
            bool result = System.Enum.TryParse(tagName,out tag);
            if (!result) { Debug.LogError("state is nothing."); }
            int stateNameHash = Animator.StringToHash(tagName);
            tagHashTable.Add(tag, stateNameHash);
        }
    }

    /// <summary>
    /// 開始
    /// </summary>
    public void Entry()
    {
        agent.isStopped = true;
        currentAnimatorTagHash = 0;
        prevAnimatorTagHash = 0;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        currentAnimatorTagHash = animator.GetCurrentAnimatorStateInfo(0).tagHash;

        if (IsMoveStartMoment()) { agent.isStopped = false; }
        if (IsMoveEndMoment())   { agent.isStopped = true; }

        prevAnimatorTagHash = currentAnimatorTagHash;
    }

    /// <summary>
    /// モーション開始直前
    /// </summary>
    /// <returns></returns>
    bool IsMoveStartMoment()
    {
        bool current = (currentAnimatorTagHash == tagHashTable[AnmationTag.MoveStart]);
        bool prev    = (prevAnimatorTagHash != tagHashTable[AnmationTag.MoveStart]);

        if (current && prev)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// モーション中
    /// </summary>
    /// <returns></returns>
    bool IsMoveLooping()
    {
        bool current = (currentAnimatorTagHash == tagHashTable[AnmationTag.MoveLoop]);
        bool prev = (prevAnimatorTagHash != tagHashTable[AnmationTag.MoveLoop]);

        if (current && prev)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// モーションの終了直前
    /// </summary>
    /// <returns></returns>
    bool IsMoveEndMoment()
    {
        bool current = (currentAnimatorTagHash == tagHashTable[AnmationTag.MoveEnd] ||
                        currentAnimatorTagHash == tagHashTable[AnmationTag.Attack]);

        bool prev = (prevAnimatorTagHash != tagHashTable[AnmationTag.MoveEnd] ||
                     prevAnimatorTagHash != tagHashTable[AnmationTag.Attack]);

        if (current && prev)
        {
            return true;
        }
        return false;
    }
}
