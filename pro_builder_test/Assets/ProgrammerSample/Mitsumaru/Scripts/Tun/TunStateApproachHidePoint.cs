using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

/// <summary>
/// プレイヤーが隠れいているハイドポイントまで接近
/// </summary>
public class TunStateApproachHidePoint : StateMachineBehaviour
{
    // ハイドポイントからツンが目の前に停止する位置までの距離
    [SerializeField]
    float lockerToTaegetDistance = 0;
    [SerializeField]
    float bedCenterRadius = 0;

    [SerializeField]
    float speed = default;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // エリアデータ管理クラス
    TunAreaDataManager areaDataManager = null;

    // ハイドコントローラー
    PlayerHideController hideController = null;

    // ツンの現在のエリアデータ
    TunAreaData areaData;
    // 出現してから一番最初に確認するハイドポイント
    GameObject firstCheckingHide = null;
    // 現在確認しているハイドポイント
    GameObject currentCheckingHide = null;
    // 現在確認しているハイドポイントのインデックス
    int currentHideIndex = -1;

    /// <summary>
    /// ステート開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュ取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
        navMesh.isStopped = false;

        // 移動速度を設定する
        navMesh.speed = speed;

        // エリアデータ管理クラスを取得
        areaDataManager = FindObjectOfType<TunAreaDataManager>() ?? areaDataManager;
        // ハイドコントローラーを取得
        hideController = FindObjectOfType<PlayerHideController>() ?? hideController;

        // 一番最初に確認するハイドポイントを取得
        firstCheckingHide = hideController.HideObj ?? firstCheckingHide;

        // ハイドポイントから属しているエリアデータを取得
        areaData = areaDataManager.GetTunAreaData(firstCheckingHide.GetInstanceID());

        // エリアデータ内のハイドポイントをリストで取得
        List<GameObject> hideObjectToList = areaData.HideObject.ToList();
        // 一番最初に確認するハイドポイントのインデックスを取得
        int firstHideIndex = hideObjectToList.IndexOf(firstCheckingHide);
        // 現在のハイドポイントのインデックス = 最初のハイドポイントのインデックス + 確認済みのハイドポイントの数
        if (currentHideIndex == -1)
        {
            currentHideIndex = firstHideIndex;
        }
        else
        {
            currentHideIndex++;
        }
        // 算出したハイドポイントのインデックスがリスト範囲外だった場合は先頭へ
        if (currentHideIndex > hideObjectToList.Count -1) { currentHideIndex = 0; }
        // 算出したインデックスからハイドポイントを取得
        currentCheckingHide = hideObjectToList[currentHideIndex];

        // ナビメッシュに目標位置をセット（ハイドポイントの目の前が目標位置になるように）
        if (currentCheckingHide.tag == "Locker")
        {
            navMesh.SetDestination(currentCheckingHide.transform.position + (currentCheckingHide.transform.forward * -1) * lockerToTaegetDistance);
        }
        else if (currentCheckingHide.tag == "Bed")
        {
            navMesh.SetDestination(currentCheckingHide.transform.position);
        }
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        bool isAngle = false;
        // ハイドポイントの位置を取得
        Vector3 hideObjPos = currentCheckingHide.transform.position;
        Vector3 hideToTun = (new Vector3(hideObjPos.x, animator.transform.position.y, hideObjPos.z) - animator.transform.position).normalized;
        // まだ目標位置に着いてなければスキップ
        if (currentCheckingHide.tag == "Locker")
        {
            if (!(navMesh.remainingDistance < navMesh.stoppingDistance)) { return; }

            navMesh.isStopped = true;

            isAngle = (Vector3.Angle(animator.transform.forward, hideToTun) < 0.1f);
        }
        else
        {
            if (!(navMesh.remainingDistance < bedCenterRadius)) { return; }
            navMesh.isStopped = true;
            isAngle = (Vector3.Angle(animator.transform.forward, hideToTun) < 0.3f);
        }
        // ハイドポイントのほうを向くようにツンを回転
        animator.transform.LookAt(new Vector3(hideObjPos.x, animator.transform.position.y, hideObjPos.z));
        // ハイドポイントに到着して、かつ向いているなら
        if (isAngle)
        {
            // ハイドポイント接近フラグをオフに
            animator.SetBool("isApproachingHide", false);

            // プレイヤーが隠れているなら
            if (animator.GetBool("isPlayerHide"))
            {
                // 隠れいているオブジェクトがツンが調べようとしているオブジェクトと同じだったら
                if (currentCheckingHide.GetInstanceID() == hideController.HideObj.GetInstanceID())
                {
                    // プレイヤーを見つけたフラグをオンに
                    animator.SetBool("isPlayerDiscover", true);
                    // ツンの攻撃トリガーをオンに
                    animator.SetTrigger("attackStart");
                }
            }
        }
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 確認済みのハイドポイントの数を増やす
        animator.SetInteger("checkedHideCount", animator.GetInteger("checkedHideCount") + 1);
        // そのエリア内のハイドポイントを全て調べたか
        if (animator.GetInteger("checkedHideCount") == areaData.HideObject.Count)
        {
            // ハイド確認終了フラグをオンにする
            animator.SetBool("isHideCheckEnd",true);
            // 値をリセット
            animator.SetInteger("checkedHideCount", 0);
        }
    }
}
