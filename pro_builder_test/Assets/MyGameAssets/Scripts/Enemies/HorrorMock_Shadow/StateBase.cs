using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// エネミーの行うステートの規定。
/// </summary>
public abstract class StateBase
{
    // パラメーター
    protected EnemyParameter parameter;
    // アニメーター
    protected Animator animator;
    // ナビメッシュエージェント
    protected NavMeshAgent agent;
    // メッシュレンダラー
    protected SkinnedMeshRenderer meshRenderer;

    public bool IsSetedNextState { get; private set; } = false;
    public int NextStateId { get; private set; } = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="animator"></param>
    /// <param name="agent"></param>
    /// <param name="meshRenderer"></param>
    public void Initialize(EnemyParameter parameter,Animator animator,NavMeshAgent agent,SkinnedMeshRenderer meshRenderer)
    {
        this.parameter = parameter;
        this.animator = animator;
        this.agent = agent;
        this.meshRenderer = meshRenderer;
    }

    /// <summary>
    /// 開始
    /// </summary>
    public virtual void Entry() { }

    /// <summary>
    /// 更新
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// 終了
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// 次のステートのIDをセット
    /// </summary>
    /// <param name="stateId"></param>
    public void SetNextState(int stateId)
    {
        NextStateId = stateId;
        IsSetedNextState = true;
    }

    /// <summary>
    /// 次のステートに変更するフラグをリセット
    /// </summary>
    public void ResetNextStateFlag()
    {
        NextStateId = -1;
        IsSetedNextState = false;
    }
}
