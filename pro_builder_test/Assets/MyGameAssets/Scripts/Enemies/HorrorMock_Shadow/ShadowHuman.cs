using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class ShadowHuman : MonoBehaviour
{
    // 影人間のステートマシン
    [SerializeField]
    StateMachine shadowStateMachine = default;

    /// <summary>
    /// 初期処理
    /// </summary>
    void Awake()
    {
        // 各クラスのインスタンスを取得
        shadowStateMachine.player = GameObject.FindGameObjectWithTag("Player");
        shadowStateMachine.soundSpawner = GameObject.FindObjectOfType<SoundAreaSpawner>();
        shadowStateMachine.damageEvent = GameObject.FindObjectOfType<PlayerDamageEvent>();

        // ステートクラスのインスタンスを生成
        StateBase[] states = new StateBase[]
        {
            new ShadowStateNormal(shadowStateMachine.soundSpawner),
            new ShadowStateCaution(),
            new ShadowStateFighting(),
        };

        // ステートマシンの初期化
        shadowStateMachine.Initialize(states);
    }

    /// <summary>
    /// 開始
    /// </summary>
    void OnEnable()
    {
        // パラメーターの初期化
        shadowStateMachine.Entry();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // ステートマシンの更新
        shadowStateMachine.Update();
    }

    /// <summary>
    /// スポーンさせる
    /// </summary>
    /// <param name="state"></param>
    public void Spawn(StateType state, Vector3 targetPosition = default)
    {
        shadowStateMachine.currentState = state;
        gameObject.SetActive(true);
        if (targetPosition != default) { shadowStateMachine.agent.SetDestination(targetPosition); }
        shadowStateMachine.animator.SetInteger("NextStateTypeId", (int)state);
        shadowStateMachine.animator.SetInteger("AnimatorStateTypeId", (int)state);
    }

    /// <summary>
    /// 現在のステートを取得
    /// </summary>
    /// <returns></returns>
    public StateType GetCurrentState()
    {
        return shadowStateMachine.currentState;
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
        shadowStateMachine.isAppear = true;
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
        shadowStateMachine.isAppear = false;
    }

    /// <summary>
    /// プレイヤーが警戒範囲に入った
    /// </summary>
    public void OnPlayerEnterToCautionRange(Collider other)
    {
        // 通常状態のみ
        //if (shadowStateMachine.currentState != StateType.Normal) { return; }
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        // 移動目標位置を発信源に
        shadowStateMachine.agent.SetDestination(other.transform.position);
        shadowStateMachine.animator.SetBool("IsAppear", true);
    }

    /// <summary>
    /// プレイヤー警戒範囲から出た
    /// </summary>
    public void OnPlayerExitToCautionRange(Collider other)
    {
        // 通常状態のみ
        //if (shadowStateMachine.currentState != StateType.Normal) { return; }
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        // 出現フラグを起こす
        shadowStateMachine.animator.SetBool("IsAppear", false);
    }

    /// <summary>
    /// 音を聞いた！
    /// </summary>
    /// <param name="other"></param>
    public void OnHeardNoise(Collider other)
    {
        if (!shadowStateMachine.parameter.IsDetectNoiseToTransparent)
        {
            // 警戒状態のみ
            if (shadowStateMachine.currentState != StateType.Caution) { return; }
        }
        // ノイズのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }

        // 移動目標位置を発信源に
        shadowStateMachine.agent.SetDestination(other.transform.position);
        shadowStateMachine.animator.SetBool("IsAppear", true);
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
        shadowStateMachine.SetNextState(StateType.Fighting);

        // プレイヤーを移動目標位置に
        shadowStateMachine.agent.SetDestination(other.transform.position);
    }

    /// <summary>
    /// プレイヤーを追いかける
    /// </summary>
    public void ChasePlayer(Collider other)
    {
        // 戦闘状態のみ
        if (shadowStateMachine.currentState != StateType.Fighting) { return; }
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        // プレイヤーを移動目標位置に
        shadowStateMachine.agent.SetDestination(other.transform.position);
    }

    /// <summary>
    /// プレイヤーに攻撃！
    /// </summary>
    /// <param name="other"></param>
    public void AttackPlayer(Collider other)
    {
        // 戦闘状態のみ
        if (shadowStateMachine.currentState != StateType.Fighting) { return; }

        // 攻撃の種類をセット
        shadowStateMachine.animator.SetInteger("AttackConditionType", shadowStateMachine.attackTypeId);
        // 攻撃トリガーをセット
        shadowStateMachine.animator.SetTrigger("Attaking");
        // 待機フラグを立てる
        shadowStateMachine.animator.SetBool("IsWaiting", true);
        // ダメージイベント
        shadowStateMachine.damageEvent.Invoke(transform,shadowStateMachine.parameter.Damage);
    }
}
