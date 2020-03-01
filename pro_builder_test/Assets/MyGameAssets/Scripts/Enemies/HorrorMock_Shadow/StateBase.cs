using UnityEngine;
using UnityEngine.AI;
using StateType = EnemyParameter.StateType;

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
    // サウンドスポナー
    protected SoundAreaSpawner soundSpawner;
    protected EnemyBase enemy;

    public bool IsSetedNextState { get; private set; } = false;
    public int NextStateId { get; private set; } = 0;
    protected bool IsInAttackRange { get; private set; }

    // 強制不透明
    public bool ForceTransparentOff { get; protected set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="animator"></param>
    /// <param name="agent"></param>
    /// <param name="meshRenderer"></param>
    public void Initialize(EnemyParameter parameter,Animator animator,NavMeshAgent agent,SkinnedMeshRenderer meshRenderer, SoundAreaSpawner sound, EnemyBase _enemy)
    {
        this.parameter = parameter;
        this.animator = animator;
        this.agent = agent;
        this.meshRenderer = meshRenderer;
        soundSpawner = sound;
        enemy = _enemy;
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
    public virtual void Exit() { 
        ResetNextStateFlag();
    }

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

    /// <summary>
    /// 音を聞いた(状態によって範囲が変わる、通常の聴覚範囲)
    /// </summary>
    public virtual void OnHearNoise(GameObject noise) { }

    /// <summary>
    /// 直接感知範囲で音を聞いた（状態によって変わらない、この範囲で一定以上の音を聞くと即座に攻撃に移動する範囲。通常の聴覚範囲より優先される）
    /// </summary>
    public virtual void OnHearNoiseAtDirectDetectRange(GameObject noise)
    {
        //////////////////
        // 警戒、通常共通
        //////////////////
        // 一定レベル以上なら即攻撃状態
        if (soundSpawner.TotalSoundLevel > parameter.DirectDetectSoundLevel)
        {
            SetNextState((int)StateType.Fighting);

            // 姿は見えていないので目標位置はノイズの位置
            var randomRange = parameter.NoiseTargetPosRandomRange;
            var noisePos = noise.transform.position + new Vector3(Random.Range(0, randomRange), 0, Random.Range(0, randomRange));
            agent.SetDestination(noisePos);
        }
    }

    /// <summary>
    /// 戦闘範囲で音を聞いた（状態によって変わらない）
    /// </summary>
    public virtual void OnHearNoiseAtFightingRange(GameObject noise) { }

    /// <summary>
    /// プレイヤーを発見した
    /// </summary>
    public virtual void OnDetectedPlayer(GameObject player)
    {
        //////////////////
        // 警戒、通常共通
        //////////////////
        // 戦闘状態に
        SetNextState((int)StateType.Fighting);

        // プレイヤーを移動目標位置に
        agent.SetDestination(player.transform.position);
    }

    /// <summary>
    /// プレイヤーを発見している視線ループ
    /// </summary>
    public virtual void OnDetectPlayerStay(GameObject player) { }

    /// <summary>
    ///  プレイヤーを見失った
    /// </summary>
    public virtual void OnMissingPlayer(GameObject player) { }

    /// <summary>
    /// 攻撃実行
    /// </summary>
    public virtual void OnAttack(){ }

    /// <summary>
    /// 攻撃範囲入った
    /// </summary>
    public void OnEnterAttackRange(GameObject player)
    {
        IsInAttackRange = true;
    }

    /// <summary>
    /// 攻撃範囲抜けた
    /// </summary>
    public void OnExitAttackRange(GameObject player)
    {
        IsInAttackRange = false;
    }
}
