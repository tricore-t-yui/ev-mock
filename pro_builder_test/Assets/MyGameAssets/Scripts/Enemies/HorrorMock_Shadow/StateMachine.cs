using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Text.RegularExpressions;
using StateType = EnemyParameter.StateType;
using NormalStateType = EnemyParameter.NormalStateType;

public class StateMachine : MonoBehaviour
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
        // 各クラスのインスタンスを取得
        player = GameObject.FindGameObjectWithTag("Player");
        damageEvent = GameObject.FindObjectOfType<PlayerDamageEvent>();

        // インスタンスを取得
        this.states = states;

        // 各ステートの初期化を行う
        System.Array.ForEach(states, state => state?.Initialize(parameter, animator, agent, meshRenderer));

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

        if (!parameter.Inverse)
        {
            // 出現フラグを倒す
            isAppear = false;
            if (parameter.IsTransparencyByDistance)
            {
                appearFadeCounter = parameter.TransparencyMin;
            }
            else
            {
                appearFadeCounter = 0;
            }
        }
        else
        {
            // 出現フラグを倒す
            isAppear = true;
            if (parameter.IsTransparencyByDistance)
            {
                appearFadeCounter = parameter.TransparencyMax;
            }
            else
            {
                appearFadeCounter = 1;
            }
        }

        states[(int)currentState].Entry();

        // 待機 or 徘徊に設定
        animator.SetBool("IsWander", (parameter.NormalState == NormalStateType.Wanderer) ? true : false);

        // ナビメッシュの移動制御クラスの開始処理
        navMeshStopingSwitcher.Entry();
    }

    public void Spawn(StateType type,Vector3 target = default)
    {
        gameObject.SetActive(true);
        currentState = type;
        animator.SetInteger("NextStateTypeId", (int)type);
        animator.SetInteger("AnimatorStateTypeId", (int)type);
        // 初期位置にワープ
        agent.Warp(parameter.InitialPosition);
        if (target != default) { agent.SetDestination(target); }
        // スポーン時の処理を行う
        Entry(parameter);
    }

    public void Spawn(StateType type,EnemyParameter parameter, Vector3 target = default)
    {
        gameObject.SetActive(true);
        currentState = type;
        animator.SetInteger("NextStateTypeId", (int)type);
        animator.SetInteger("AnimatorStateTypeId", (int)type);
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
            if (!parameter.Inverse)
            {
                if (isAppear)
                {
                    appearFadeCounter += parameter.AppearFadeTime;
                }
                // 出現フラグ折れてるうう
                else
                {
                    appearFadeCounter -= parameter.AppearFadeTime;
                }
            }
            else
            {
                if (!isAppear)
                {
                    appearFadeCounter += parameter.AppearFadeTime;
                }
                // 出現フラグ折れてるうう
                else
                {
                    appearFadeCounter -= parameter.AppearFadeTime;
                }
            }
        }
        else
        {
            if (!parameter.Inverse)
            {
                // プレイヤーとの距離によって影人間の透明度を変える
                float dist = (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position).magnitude;

                // プレイヤーとの距離を０～１に丸め込む
                dist = dist / parameter.StateRanges[(int)currentState].appear * ((parameter.TransparencyMax - parameter.TransparencyMin));
                appearFadeCounter = (parameter.TransparencyMax - parameter.TransparencyMin) - dist;
            }
            else
            {
                // プレイヤーとの距離によって影人間の透明度を変える
                float dist = (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position).magnitude;

                // プレイヤーとの距離を０～１に丸め込む
                dist = dist / parameter.StateRanges[(int)currentState].appear * ((parameter.TransparencyMax - parameter.TransparencyMin));
                appearFadeCounter = parameter.TransparencyMin + dist;
            }
        }
        // 現在の透明度が最小と最大を超えないように補正
        if (parameter.IsTransparencyByDistance)
        {
            appearFadeCounter = Mathf.Clamp(appearFadeCounter, parameter.TransparencyMin, parameter.TransparencyMax);
        }
        else
        {
            appearFadeCounter = Mathf.Clamp(appearFadeCounter, 0, 1);
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
        // 影人間とプレイヤーとの距離が一定以内かどうか
        if ((player.transform.position - agent.transform.position).magnitude < parameter.DisappearDistance)
        {
            // 一定距離近づいたら消える
            if (parameter.IsApproachedDisappear)
            {
                agent.gameObject.SetActive(false);
                animator.gameObject.SetActive(parameter.IsRespawn ? true : false);
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
    /// 音を聞いた！
    /// </summary>
    /// <param name="other"></param>
    public void OnHeardNoise(Collider other)
    {
        if (!parameter.IsDetectNoiseToTransparent)
        {
            // 見えている
            if (!isAppear) { return; }
        }
        // ノイズのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }

        // 移動目標位置を発信源に
        agent.SetDestination(other.transform.position);

        // 音を聞いた
        animator.SetBool("IsDetectedNoise", true);
    }

    /// <summary>
    /// プレイヤーを発見した！
    /// </summary>
    /// <param name="other"></param>
    public void OnDetectedPlayer(Collider other)
    {
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        // 戦闘状態に
        SetNextState(StateType.Fighting);

        // プレイヤーを移動目標位置に
        agent.SetDestination(other.transform.position);
    }

    /// <summary>
    /// プレイヤーを追いかける
    /// </summary>
    public void ChasePlayer(Collider other)
    {
        // 戦闘状態のみ
        if (currentState != StateType.Fighting) { return; }
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        // プレイヤーを移動目標位置に
        agent.SetDestination(other.transform.position);
    }

    /// <summary>
    /// プレイヤーに攻撃！
    /// </summary>
    /// <param name="other"></param>
    public void AttackPlayer(Collider other)
    {
        // 戦闘状態のみ
        if (currentState != StateType.Fighting) { return; }

        // 攻撃の種類をセット
        animator.SetInteger("AttackConditionType", attackTypeId);
        // 攻撃トリガーをセット
        animator.SetTrigger("Attaking");
        // 待機フラグを立てる
        animator.SetBool("IsWaiting", true);
        // ダメージイベント
        damageEvent.Invoke(transform, parameter.Damage);
    }
}
