using UnityEngine;
using UnityEngine.AI;
using StateType = EnemyParameter.StateType;
using NormalStateType = EnemyParameter.NormalStateType;

/// <summary>
/// ツンとエネミーの共通処理
/// </summary>
public class EnemyBase : MonoBehaviour
{
    // 敵のパラメーター
    [SerializeField]
    protected EnemyParameter parameter = default;

    // アニメーター
    [SerializeField]
    protected Animator animator = default;

    // ナビメッシュ
    [SerializeField]
    protected NavMeshAgent agent = default;

    // メッシュレンダラー
    [SerializeField]
    protected SkinnedMeshRenderer meshRenderer = default;

    // ナビメッシュの移動制御
    NavMeshStopingSwitcher navMeshStopingSwitcher = new NavMeshStopingSwitcher();

    // 現在のステート
    public StateType currentState { get; protected set; } = StateType.Normal;

    // ステート変更トリガー
    bool stateChangeTrigger = false;
    // 次のステートのiD
    int nextStateId = -1;

    // 出現フラグ
    protected bool isAppear = false;
    // 出現フェード
    float appearAlpha = 0;

    // 攻撃の種類
    protected int attackTypeId = 0;

    // プレイヤー
    protected GameObject player = default;

    // サウンドスポナー
    protected SoundAreaSpawner soundSpawner = default;

    // ダメージイベント
    protected PlayerDamageEvent damageEvent = default;

    // ステート
    protected StateBase[] states;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(StateBase[] states)
    {
        soundSpawner = GameObject.FindObjectOfType<SoundAreaSpawner>();

        // 各クラスのインスタンスを取得
        player = GameObject.FindGameObjectWithTag("Player");
        damageEvent = GameObject.FindObjectOfType<PlayerDamageEvent>();

        // インスタンスを取得
        this.states = states;

        // 各ステートの初期化を行う
        System.Array.ForEach(states, state => state?.Initialize(parameter, animator, agent, meshRenderer, soundSpawner, this));

        // パラメーターの初期化
        parameter.Initialize();

        // ナビメッシュの移動制御クラスの初期化
        navMeshStopingSwitcher.Initialize(animator, agent);

        // ステートの遷移を行うかどうか
        animator.SetBool("IsStaticState", parameter.IsStaticState);
    }

    void Start()
    {
        Entry(parameter);
    }

    /// <summary>
    /// 開始
    /// </summary>
    public void Entry(EnemyParameter parameter)
    {
        // 範囲の初期化
        parameter.ChangeRangeRadius(currentState);

        if (parameter.IsTransparencyByDistance)
        {
            // 出現フラグを倒す
            isAppear = false;
            appearAlpha = parameter.InverseTransparency ? 0 : 1;
        }
        else
        {
            appearAlpha = 1;
        }

        states[(int)currentState].Entry();

        // 待機 or 徘徊に設定
        animator.SetBool("IsWander", (parameter.NormalState == NormalStateType.Wanderer) ? true : false);

        // ナビメッシュの移動制御クラスの開始処理
        navMeshStopingSwitcher.Entry();
    }

    public void Spawn(StateType type,Vector3 target = default, Vector3 spawnPos = default)
    {
        gameObject.SetActive(true);
        currentState = type;
        animator.SetInteger("NextStateTypeId", (int)type);
        animator.SetInteger("AnimatorStateTypeId", (int)type);
        // 初期位置にワープ
        if (spawnPos == default) { agent.Warp(parameter.InitialPosition); transform.rotation = parameter.InitialRotation; }
        else                     { agent.Warp(spawnPos); }
        if (target != default) { agent.SetDestination(target); }
        // スポーン時の処理を行う
        Entry(parameter);
    }

    public void Spawn(StateType type,EnemyParameter parameter, Vector3 target = default, Vector3 spawnPos = default)
    {
        gameObject.SetActive(true);
        currentState = type;
        animator.SetInteger("NextStateTypeId", (int)type);
        animator.SetInteger("AnimatorStateTypeId", (int)type);
        if (spawnPos == default) { agent.Warp(parameter.InitialPosition); transform.rotation = parameter.InitialRotation; }
        else                     { agent.Warp(spawnPos); }
        if (target != default) { agent.SetDestination(target); }
        // スポーン時の処理を行う
        Entry(parameter);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        UpdateTransparent();
        if (parameter.IsStaticState) return;

        // ステートの更新
        states[(int)currentState].Update();

        // ステート変更のフラグがたった
        if (states[(int)currentState].IsSetedNextState)
        {
            stateChangeTrigger = true;
            nextStateId = states[(int)currentState].NextStateId;
            states[(int)currentState].ResetNextStateFlag();
        }

        if (stateChangeTrigger)
        {
            // 次のステートをセット
            SetNextState((StateType)nextStateId);

            // アニメーター側のステートの変更が完了した
            if (animator.GetInteger("NextStateTypeId") ==
                animator.GetInteger("AnimatorStateTypeId"))
            {
                // ステートを変更する
                ChangeNextState((StateType)nextStateId);
                // 範囲のサイズを変更
                parameter.ChangeRangeRadius((StateType)nextStateId);
                stateChangeTrigger = false;
            }
        }

        // ナビメッシュの移動制御クラスの更新
        navMeshStopingSwitcher.Update();

        // ナビメッシュのターゲットが一定以上近くにいたらローテーション補完
        var toTarget = agent.destination - transform.position;
        if (toTarget.magnitude < 2.0f && toTarget.magnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(toTarget.normalized), 0.1f);
        }

        // 特殊アクションの制御
        ControlSpecialAction();

        // 移動速度をパラメータに反映
        animator.SetFloat("CurrentMoveSpeed", agent.speed);
    }

    /// <summary>
    /// 特殊アクションの制御
    /// </summary>
    public void ControlSpecialAction()
    {
        if (parameter.IsStaticState) return;
        // 一定距離近づいたら消える
        if (parameter.IsApproachedDisappear)
        {
            // 影人間とプレイヤーとの距離が一定以内かどうか
            if ((player.transform.position - agent.transform.position).magnitude < parameter.DisappearDistance)
            {
                agent.gameObject.SetActive(false);
            }
        }

        // カメラからフェードアウトで消える
        if (parameter.IsCameraFadeOutDisappear)
        {
            if (!meshRenderer.isVisible)
            {
                agent.gameObject.SetActive(false);
                animator.gameObject.SetActive(parameter.IsRespawn ? true : false);
            }
        }
    }

    /// <summary>
    /// 次のステートをセット
    /// </summary>
    /// <param name="type"></param>
    void SetNextState(StateType type)
    {
        if (parameter.IsStaticState) return;
        animator.SetInteger("NextStateTypeId", (int)type);
        nextStateId = (int)type;
        stateChangeTrigger = true;
    }

    /// <summary>
    /// 次のステートに変更
    /// </summary>
    /// <param name="state"></param>
    void ChangeNextState(StateType type)
    {
        if (parameter.IsStaticState) return;
        states[(int)currentState].Exit();
        currentState = type;
        states[(int)currentState].Entry();
    }

    /// <summary>
    /// 透明度変更
    /// </summary>
    void UpdateTransparent()
    {
        // 出現フラグが起きた
        if (!parameter.IsTransparencyByDistance || states[(int)currentState].ForceTransparentOff)
        {
            appearAlpha = 1;
        }
        else
        {
            // プレイヤーとの距離によって影人間の透明度を変える
            float dist = (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position).magnitude;

            // 完全可視距離の距離レート
            if (parameter.RangeParameter.fullAppear > parameter.RangeParameter.appear) Debug.LogError("完全可視距離が可視距離より大きいです");
            float finalRate = 1.0f;
            if (dist > parameter.RangeParameter.appear)
            {
                finalRate = 0;
            }
            else if(dist > parameter.RangeParameter.fullAppear)
            {
                // 完全可視距離をゼロとして0～1.0の距離でレート作成
                var rate = (dist - parameter.RangeParameter.fullAppear) / (parameter.RangeParameter.appear - parameter.RangeParameter.fullAppear);
                finalRate = 1.0f - rate;
            }

            appearAlpha = parameter.InverseTransparency ? -finalRate : finalRate;
        }
        appearAlpha = Mathf.Clamp(appearAlpha, parameter.TransparencyMin, parameter.TransparencyMax);

        // 透明度をメッシュに反映
        if(parameter.IsTransparencyByDistance)
        {
            Color result = new Color(
                        meshRenderer.material.color.r,
                        meshRenderer.material.color.g,
                        meshRenderer.material.color.b,
                        appearAlpha);
            meshRenderer.material.color = result;
        }
    }

    /// <summary>
    /// プレイヤーに攻撃。モーション入れば確定ヒットとする。アニメーションイベント
    /// </summary>
    public void AttackPlayerByAnimation()
    {
        // 攻撃の種類をセット
        animator.SetInteger("AttackConditionType", 0);  // NOTE: yui-t 引きずりだし攻撃は一旦プレイヤーに判断させる
        // ダメージイベント
        damageEvent.Invoke(transform, parameter.Damage);
        states[(int)currentState].OnAttack();
    }

    /// <summary>
    /// プレイヤーが見える範囲に入った
    /// </summary>
    public void OnPlayerEnterToAppearRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // 通常状態のみ
        //if (shadowStateMachine.currentState != StateType.Normal) { return; }
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        // 出現フラグを起こす
        isAppear = true;
    }

    /// <summary>
    /// プレイヤー見える範囲から出た
    /// </summary>
    public void OnPlayerExitToAppearRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // 通常状態のみ
        //if (shadowStateMachine.currentState != StateType.Normal) { return; }
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        // 出現フラグを起こす
        isAppear = false;
    }

    /// <summary>
    /// 聴覚範囲に入った
    /// </summary>
    public void OnEnterNoiseHearRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // ノイズのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }
        states[(int)currentState].OnHearNoise(other.gameObject);
    }

    /// <summary>
    /// 直接攻撃移行聴覚範囲に入った
    /// </summary>
    public void OnEnterDirectDetectRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // ノイズのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }
        states[(int)currentState].OnHearNoiseAtDirectDetectRange(other.gameObject);
    }

    /// <summary>
    /// 戦闘範囲内
    /// </summary>
    public void OnEnterFightingRange(Collider other)
    {        
        if (parameter.IsStaticState) return;
        // ノイズのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }
        states[(int)currentState].OnHearNoiseAtFightingRange(other.gameObject);
    }

    /// <summary>
    /// ビューレンジ入った
    /// </summary>
    public void OnEnterViewRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        states[(int)currentState].OnDetectedPlayer(other.gameObject);
    }

    /// <summary>
    /// ビューレンジ更新
    /// </summary>
    public void OnStayViewRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        states[(int)currentState].OnDetectPlayerStay(other.gameObject);
    }

    /// <summary>
    /// ビューレンジ抜けた
    /// </summary>
    /// <param name="other"></param>
    public void OnExitViewRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        states[(int)currentState].OnMissingPlayer(other.gameObject);
    }

    /// <summary>
    /// 攻撃範囲内
    /// </summary>
    public void OnEnterAttackRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        states[(int)currentState].OnEnterAttackRange(other.gameObject);
    }
    public void OnExitAttackRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        states[(int)currentState].OnExitAttackRange(other.gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        parameter.ChangeRangeRadius(currentState);

        // ターゲット
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(agent.destination, 0.2f);
    }
#endif
}
