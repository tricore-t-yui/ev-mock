using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowStateBase
{
    // 影人間パラメータ
    protected ShadowParameter parameter = default;
    // メッシュ
    protected SkinnedMeshRenderer meshRenderer = default;
    // アニメーター
    protected Animator animator = default;
    // ナビメッシュエージェント
    protected NavMeshAgent agent = default;

    public int  NextStateId { get; private set; } = -1;
    public bool StateChangeTrigger { get; private set; } = false;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="agent"></param>
    public void Initialize(ShadowParameter parameter,SkinnedMeshRenderer meshRenderer,Animator animator,NavMeshAgent agent)
    {
        this.parameter = parameter;
        this.meshRenderer = meshRenderer;
        this.animator = animator;
        this.agent = agent;
    }

    /// <summary>
    /// 開始
    /// </summary>
    public virtual void Entry()
    {

    }

    /// <summary>
    /// 更新
    /// </summary>
    public virtual void Update()
    {

    }

    /// <summary>
    /// 終了
    /// </summary>
    public virtual void Exit()
    {

    }

    /// <summary>
    /// 次のステートをセット
    /// </summary>
    /// <param name="stateId"></param>
    public virtual void SetNextState(int stateId)
    {
        StateChangeTrigger = true;
        NextStateId = stateId;
    }

    /// <summary>
    /// ステート変更トリガーをリセット
    /// </summary>
    /// <returns></returns>
    public void ResetTrigger()
    {
        StateChangeTrigger = false;
    }
}
