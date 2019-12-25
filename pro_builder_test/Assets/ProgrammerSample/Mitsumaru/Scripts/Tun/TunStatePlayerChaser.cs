using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class TunStatePlayerChaser : StateMachineBehaviour
{
    // 移動速度
    [SerializeField]
    float speed = 0;

    [SerializeField]
    float attackRadius = 0;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // プレイヤー
    Transform player = default;

    // エリアデータ管理クラス
    TunAreaDataManager areaDataManager = null;

    // ハイドコントローラー
    PlayerHideController hideController = null;

    // 敵のサウンドプレイヤー取得
    EnemySoundPlayer soundPlayer = null;

    TunAreaData currentArea = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // エリアデータ管理クラスを取得
        areaDataManager = FindObjectOfType<TunAreaDataManager>() ?? areaDataManager;
        // ハイドコントローラーを取得
        hideController = FindObjectOfType<PlayerHideController>() ?? hideController;

        // サウンドプレイヤー
        soundPlayer = animator.GetComponentInChildren<EnemySoundPlayer>() ?? soundPlayer;

        soundPlayer.Play("Mitsuketa");

        // 現在のエリアを取得
        currentArea = areaDataManager.GetTunAreaData(hideController.HideObj.GetInstanceID());

        // 移動速度をセット
        navMesh.speed = speed;

        navMesh.isStopped = false;

        player = GameObject.Find("Player").transform;


    }

    /// <summary>
    /// プレイヤーの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        navMesh.isStopped = false;
        // プレイヤーの位置を取得する
        navMesh.SetDestination(hideController.transform.position);

        if (navMesh.remainingDistance < attackRadius)
        {
            animator.SetTrigger("attackStart");
        }

        if (!currentArea.IsBoudsContains(player.position))
        {
            animator.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) { }
}