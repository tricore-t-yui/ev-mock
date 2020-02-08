using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ShadowState = ShadowParameter.StateType;
using NormalStateType = ShadowParameter.NormalStateType;

public class ShadowManager : MonoBehaviour
{
    [SerializeField]
    ShadowParameter parameter = default;
    [SerializeField]
    SkinnedMeshRenderer meshRenderer = default;
    [SerializeField]
    Animator animator = default;
    [SerializeField]
    NavMeshAgent agent = default;

    ShadowState currentState = ShadowState.Normal;
    SoundAreaSpawner soundSpawner = default;
    PlayerDamageEvent damageEvent = default;

    bool stateChangeTrigger = false;
    public int nextStateId = -1;

    ShadowStateBase[] shadowStates;

    /// <summary>
    /// 開始
    /// </summary>
    void Awake()
    {
        currentState = ShadowState.Normal;
        // サウンドエリアスポナーを取得
        soundSpawner = FindObjectOfType<SoundAreaSpawner>();
        if (soundSpawner == null) { Debug.LogError("soundAreaSpawner is null"); }

        // ダメージイベント
        damageEvent = FindObjectOfType<PlayerDamageEvent>();

        shadowStates = new ShadowStateBase[]
        {
            new ShadowStateNormal(),
            new ShadowStateAlert(soundSpawner),
            new ShadowStateCaution(),
            new ShadowStateFighting(),
        };

        for (int i = 0; i < shadowStates.Length; i++)
        {
            // 初期化
            shadowStates[i].Initialize(parameter,meshRenderer,animator, agent);
        }

        animator.SetBool("IsStaticState", parameter.IsStaticState);
        animator.SetBool("IsWander", (parameter.NormalState == NormalStateType.Wanderer) ? true : false);
    }

    void OnEnable()
    {
        currentState = ShadowState.Normal;
        shadowStates[(int)currentState].Entry();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // ステートの更新
        shadowStates[(int)currentState].Update();

        // ステート変更フラグがたった
        if (shadowStates[(int)currentState].StateChangeTrigger)
        {
            stateChangeTrigger = true;
            nextStateId = shadowStates[(int)currentState].NextStateId;
            // ステート変更フラグを倒す
            shadowStates[(int)currentState].ResetTrigger();
        }
        if (stateChangeTrigger)
        {
            SetNextState((ShadowState)nextStateId);

            // アニメーター側のステートの変更が完了した
            if (animator.GetInteger("NextStateTypeId") == animator.GetInteger("AnimatorStateTypeId"))
            {
                // ステートを変更する
                ChangeState((ShadowState)nextStateId);
                stateChangeTrigger = false;
            }
        }
        // 移動速度をアニメーターのパラメーターに反映
        animator.SetFloat("CurrentMoveSpeed", agent.speed);
    }

    /// <summary>
    /// 次のステートのIDをセット
    /// </summary>
    public void SetNextState(ShadowState state)
    {
        animator.SetInteger("NextStateTypeId", (int)state);
        nextStateId = (int)state;
        stateChangeTrigger = true;
    }

    /// <summary>
    /// ステートの変更
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(ShadowState state)
    {
        shadowStates[(int)currentState].Exit();
        currentState = state;
        shadowStates[(int)currentState].Entry();
    }

    /// <summary>
    /// 警戒状態に入った
    /// </summary>
    public void OnCautionRangeEnter(Collider other)
    {
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        // 通常状態のみ
        if (currentState != ShadowState.Normal) { return; }
        // 注意状態に変更
        SetNextState(ShadowState.Alert);
    }

    /// <summary>
    /// 音が聞こえた
    /// </summary>
    public void OnHeardNoise(Collider other)
    {
        // ノイズのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }
        // 警戒状態のみ
        if (currentState != ShadowState.Caution) { return; }
        // 音の発信源を目的地に設定
        agent.SetDestination(other.transform.position);
    }

    /// <summary>
    /// プレイヤーを見つけた
    /// </summary>
    /// <param name="other"></param>
    public void OnDetectPlayer(Collider other)
    {
        SetNextState(ShadowState.Fighting);
    }

    public void ChasePlayer(Collider other)
    {
        if (currentState != ShadowState.Fighting) { return; }
        agent.SetDestination(other.transform.position);
    }

    public void OnAttackRangeEnter(Collider other)
    {
        // 戦闘状態のみ
        if (currentState != ShadowState.Fighting) { return; }
        animator.SetTrigger("Attaking");
        damageEvent.Invoke(transform, 0);
        animator.SetBool("IsWaiting", true);
    }
}
