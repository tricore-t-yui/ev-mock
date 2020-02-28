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

    // サウンドスポナー
    [SerializeField]
    ShadowCrySoundSpawner crySoundSpawner = default;

    [SerializeField]
    EnemySoundPlayer soundPlayer = default;

    // 現在のステート
    public StateType currentState { get; protected set; } = StateType.Normal;

    // ステート変更トリガー
    bool stateChangeTrigger = false;
    // 次のステートのiD
    int nextStateId = -1;

    // 出現フラグ
    protected bool isAppear = false;
    // 出現フェード
    float appearFadeCounter = 0;

    // 攻撃の種類
    protected int attackTypeId = 0;

    // 待機カウンター
    float disappearWaitCounter = 0;

    // 待機フラグ
    bool isApproached = false;

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
            if (!parameter.InverseTransparency)
            {
                // 出現フラグを倒す
                isAppear = false;
                appearFadeCounter = parameter.TransparencyMin;
            }
            else
            {
                // 出現フラグを倒す
                isAppear = true;
                appearFadeCounter = parameter.TransparencyMax;
            }
        }
        else
        {
            appearFadeCounter = 1;
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
        if (spawnPos == default) { agent.Warp(parameter.InitialPosition); }
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
        if (spawnPos == default) { agent.Warp(parameter.InitialPosition); }
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

        // 出現フラグが起きた
        if (!parameter.IsTransparencyByDistance)
        {
            appearFadeCounter = 1;
        }
        else
        {
            if (!parameter.InverseTransparency)
            {
                // プレイヤーとの距離によって影人間の透明度を変える
                float dist = (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position).magnitude;

                // プレイヤーとの距離を０～１に丸め込む
                dist = dist / parameter.RangeParameter.appear * ((parameter.TransparencyMax - parameter.TransparencyMin));
                appearFadeCounter = (parameter.TransparencyMax - parameter.TransparencyMin) - dist;
            }
            else
            {
                // プレイヤーとの距離によって影人間の透明度を変える
                float dist = (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position).magnitude;

                // プレイヤーとの距離を０～１に丸め込む
                dist = dist / parameter.RangeParameter.appear * ((parameter.TransparencyMax - parameter.TransparencyMin));
                appearFadeCounter = parameter.TransparencyMin + dist;
            }
        }
        // 現在の透明度が最小と最大を超えないように補正
        if (parameter.IsTransparencyByDistance)
        {
            appearFadeCounter = Mathf.Clamp(appearFadeCounter, parameter.TransparencyMin, parameter.TransparencyMax);
        }

        // 透明度をメッシュに反映
        Color result = new Color(
                    meshRenderer.material.color.r,
                    meshRenderer.material.color.g,
                    meshRenderer.material.color.b,
                    appearFadeCounter);
        meshRenderer.material.color = result;

        // ナビメッシュの移動制御クラスの更新
        navMeshStopingSwitcher.Update();

        // 特殊アクションの制御
        ControlSpecialAction();

        // 移動速度をパラメータに反映
        animator.SetFloat("CurrentMoveSpeed", agent.speed);

        // 戦闘状態時にサウンドスポナーをオンにする
        if (crySoundSpawner != null)
        {
            if (currentState == StateType.Fighting)
            {
                if (!crySoundSpawner.gameObject.activeSelf)
                {
                    crySoundSpawner.gameObject.SetActive(true);
                }
            }
            else
            {
                if (crySoundSpawner.gameObject.activeSelf)
                {
                    crySoundSpawner.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// 特殊アクションの制御
    /// </summary>
    public void ControlSpecialAction()
    {
        // 一定距離近づいたら消える
        if (parameter.IsApproachedDisappear)
        {
            // 影人間とプレイヤーとの距離が一定以内かどうか
            if ((player.transform.position - agent.transform.position).magnitude < parameter.DisappearDistance)
            {
                if (!parameter.IsDisappearWait)
                {
                    disappearWaitCounter = parameter.DisappearWaitTime;
                }

                // 待機する
                animator.SetBool("IsWaiting", parameter.IsDisappearWait ? true : false);
                isApproached = true;
            }

            // 一定時間待機したら
            if (isApproached)
            {
                // 待機する
                animator.SetBool("IsWaiting", parameter.IsDisappearWait ? true : false);
                disappearWaitCounter += Time.deltaTime;
                if (disappearWaitCounter > parameter.DisappearWaitTime)
                {
                    isAppear = false;
                    appearFadeCounter -= parameter.AppearFadeTime;
                    if (appearFadeCounter < 0.001f)
                    {
                        // 待機解除
                        agent.gameObject.SetActive(false);
                        isApproached = false;
                        animator.gameObject.SetActive(parameter.IsRespawn ? true : false);
                        disappearWaitCounter = 0;
                    }
                }
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
    public void SetNextState(StateType type)
    {
        animator.SetInteger("NextStateTypeId", (int)type);
        nextStateId = (int)type;
        stateChangeTrigger = true;
    }

    /// <summary>
    /// 次のステートに変更
    /// </summary>
    /// <param name="state"></param>
    public void ChangeNextState(StateType type)
    {
        states[(int)currentState].Exit();
        currentState = type;
        states[(int)currentState].Entry();
    }

    /// <summary>
    /// プレイヤーに攻撃。モーション入れば確定ヒットとする
    /// </summary>
    public void AttackPlayer(Collider other)
    {
        // 攻撃の種類をセット
        animator.SetInteger("AttackConditionType", 0);
        // ダメージイベント
        damageEvent.Invoke(transform, parameter.Damage);
    }

    /// <summary>
    /// プレイヤーが見える範囲に入った
    /// </summary>
    public void OnPlayerEnterToAppearRange(Collider other)
    {
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
        // ノイズのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }
        states[(int)currentState].OnHearNoise(other.gameObject);
    }

    /// <summary>
    /// 直接攻撃移行聴覚範囲に入った
    /// </summary>
    public void OnEnterDirectDetectRange(Collider other)
    {
        // ノイズのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }
        states[(int)currentState].OnHearNoiseAtDirectDetectRange(other.gameObject);
    }

    /// <summary>
    /// 戦闘範囲内
    /// </summary>
    public void OnEnterFightingRange(Collider other)
    {        
        // ノイズのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }
        states[(int)currentState].OnHearNoiseAtFightingRange(other.gameObject);
    }

    /// <summary>
    /// ビューレンジ入った
    /// </summary>
    public void OnEnterViewRange(Collider other)
    {
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        states[(int)currentState].OnDetectedPlayer(other.gameObject);
    }

    /// <summary>
    /// ビューレンジ抜けた
    /// </summary>
    /// <param name="other"></param>
    public void OnExitViewRange(Collider other)
    {
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        states[(int)currentState].OnMissingPlayer(other.gameObject);
    }
    
    /// <summary>
    /// 攻撃範囲内
    /// </summary>
    public void OnEnterAttackRange(Collider other)
    {
        animator.SetBool("InAttackRange", true);
    }
    public void OnExitAttackRange(Collider other)
    {
        animator.SetBool("InAttackRange", false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        parameter.ChangeRangeRadius(currentState);
    }
#endif
}
