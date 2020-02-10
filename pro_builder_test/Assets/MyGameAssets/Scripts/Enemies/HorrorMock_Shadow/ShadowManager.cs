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
    GameObject player = default;
    SoundAreaSpawner soundSpawner = default;
    PlayerDamageEvent damageEvent = default;

    bool stateChangeTrigger = false;
    public int nextStateId = -1;

    float appearFadeCounter = 0;

    ShadowStateBase[] shadowStates;

    /// <summary>
    /// 開始
    /// </summary>
    void Awake()
    {
        parameter.Initialize();
        currentState = ShadowState.Normal;
        // サウンドエリアスポナーを取得
        soundSpawner = FindObjectOfType<SoundAreaSpawner>();
        if (soundSpawner == null) { Debug.LogError("soundAreaSpawner is null"); }

        // ダメージイベント
        damageEvent = FindObjectOfType<PlayerDamageEvent>();

        // プレイヤー
        player = GameObject.FindGameObjectWithTag("Player");

        shadowStates = new ShadowStateBase[]
        {
            new ShadowStateNormal(soundSpawner),
            new ShadowStateCaution(),
            new ShadowStateFighting(),
        };

        for (int i = 0; i < shadowStates.Length; i++)
        {
            // 初期化
            shadowStates[i].Initialize(parameter,meshRenderer,animator, agent);
        }
    }

    void OnEnable()
    {
        currentState = ShadowState.Normal;
        shadowStates[(int)currentState].Entry();
        agent.Warp(parameter.InitialPosition);
        animator.SetBool("IsStaticState", parameter.IsStaticState);
        animator.SetBool("IsWander", (parameter.NormalState == NormalStateType.Wanderer) ? true : false);
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

        if ((player.transform.position - transform.position).magnitude < parameter.DisappearDistance)
        {
            if (parameter.IsApproachedDisappear)
            {
                gameObject.SetActive(false);
                animator.gameObject.SetActive(parameter.IsRespawn ? true : false);
            }

            if (parameter.IsCameraFadeOutDisappear)
            {
                if (!meshRenderer.isVisible)
                {
                    gameObject.SetActive(false);
                    animator.gameObject.SetActive(parameter.IsRespawn ? true : false);
                }
            }
        }

        // 見える範囲を判定
        parameter.ChangeStateRangeRadius(currentState);
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
    /// 見える範囲に入った
    /// </summary>
    /// <param name="other"></param>
    public void OnAppearRangeEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
    }

    /// <summary>
    /// 見える範囲に入り続けている
    /// </summary>
    /// <param name="other"></param>
    public void OnAppearRangeStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        meshRenderer.enabled = true;
        appearFadeCounter += parameter.AppearFadeTime;
        if (appearFadeCounter < 1)
        {
            appearFadeCounter += parameter.AppearFadeTime;
            meshRenderer.material.color = new Color(
                meshRenderer.material.color.r,
                meshRenderer.material.color.g,
                meshRenderer.material.color.b,
                appearFadeCounter);
        }
    }

    /// <summary>
    /// 見える範囲から出た
    /// </summary>
    /// <param name="other"></param>
    public void OnAppearRangeExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        appearFadeCounter = 0;
        meshRenderer.enabled = false;
    }

    /// <summary>
    /// 警戒範囲に入った
    /// </summary>
    /// <param name="other"></param>
    public void OnCautionRangeEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player") &&
            other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }

        if (currentState != ShadowState.Normal) { return; }

        animator.SetBool("IsCautionEnter", true);
    }

    /// <summary>
    /// 警戒範囲から出た
    /// </summary>
    /// <param name="other"></param>
    public void OnCautionRangeExit(Collider other)
    {
        animator.SetBool("IsCautionEnter", false);
    }

    /// <summary>
    /// 音を聞いた
    /// </summary>
    /// <param name="other"></param>
    public void OnHeardNoise(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player") &&
            other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }

        if (currentState != ShadowState.Caution) { return; }

        agent.SetDestination(other.transform.position);
    }

    public void OnDetectPlayer(Collider other)
    {
        SetNextState(ShadowState.Fighting);
    }

    public void ChasePlayer(Collider other)
    {
        if (currentState != ShadowState.Fighting) { return; }
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

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
