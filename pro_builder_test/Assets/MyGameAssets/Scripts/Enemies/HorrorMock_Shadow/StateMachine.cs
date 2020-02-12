using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateType = EnemyParameter.StateType;
using NormalStateType = EnemyParameter.NormalStateType;

[System.Serializable]
public class StateMachine
{
    public EnemyParameter parameter = default;
    public Animator animator = default;
    public NavMeshAgent agent = default;
    public SkinnedMeshRenderer meshRenderer = default;

    public StateType InitialState = default;
    [HideInInspector]
    public StateType currentState = default;

    bool stateChangeTrigger = false;
    int nextStateId = -1;

    [HideInInspector]
    float spawnCounter = 0;

    [HideInInspector]
    public bool isAppear = false;
    float appearFadeCounter = 0;

    [HideInInspector]
    public int attackTypeId = 0;

    [HideInInspector]
    public GameObject player = default;

    [HideInInspector]
    public SoundAreaSpawner soundSpawner = default;

    [HideInInspector]
    public PlayerDamageEvent damageEvent = default;

    [HideInInspector]
    public StateBase[] states;

    // ナビメッシュの移動制御
    NavMeshStopingSwitcher navMeshStopingSwitcher = new NavMeshStopingSwitcher();

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(StateBase[] states)
    {
        // インスタンスを取得
        this.states = states;

        // 各ステートの初期化を行う
        System.Array.ForEach(states, state => state?.Initialize(parameter, animator, agent, meshRenderer));

        // パラメーターの初期化
        parameter.Initialize();

        // ナビメッシュの移動制御クラスの初期化
        navMeshStopingSwitcher.Initialize(animator, agent);
    }

    /// <summary>
    /// 開始
    /// </summary>
    public void Entry()
    {
        // 初期ステートを設定
        SetNextState(InitialState);
        // 範囲の初期化
        parameter.ChangeRangeRadius(currentState);
        // 初期位置にワープ
        agent.Warp(parameter.InitialPosition);

        // 透明状態で出現させるとき
        if (!parameter.IsAlwaysAppear)
        {
            // 出現フラグを倒す
            isAppear = false;
            appearFadeCounter = 1;
        }
        // そうでなければ
        else
        {
            // 出現フラグを倒す
            isAppear = true;
            appearFadeCounter = 0;
        }
        states[(int)currentState].Entry();

        // パスのリセット
        agent.ResetPath();
        // 待機 or 徘徊に設定
        animator.SetBool("IsWander", (parameter.NormalState == NormalStateType.Wanderer) ? true : false);

        // ナビメッシュの移動制御クラスの開始処理
        navMeshStopingSwitcher.Entry();
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
        if (!parameter.IsAlwaysAppear)
        {
            if (isAppear)
            {
                if (appearFadeCounter < 1)
                {
                    appearFadeCounter += parameter.AppearFadeTime;
                    Color result = new Color(
                        meshRenderer.material.color.r,
                        meshRenderer.material.color.g,
                        meshRenderer.material.color.b,
                        appearFadeCounter);
                    meshRenderer.material.color = result;
                }
            }
            // 出現フラグ折れてるうう
            else
            {
                if (appearFadeCounter > 0)
                {
                    appearFadeCounter -= parameter.AppearFadeTime;
                    Color result = new Color(
                        meshRenderer.material.color.r,
                        meshRenderer.material.color.g,
                        meshRenderer.material.color.b,
                        appearFadeCounter);
                    meshRenderer.material.color = result;
                }
            }
        }

        // ナビメッシュの移動制御クラスの更新
        navMeshStopingSwitcher.Update();

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
}
