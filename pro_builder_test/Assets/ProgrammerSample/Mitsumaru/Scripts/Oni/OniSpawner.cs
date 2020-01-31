using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OniSpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("デスポーン、スポーンを行うキャラクターを設定する。")]
    NavMeshAgent targetCharacter = default;

    [SerializeField]
    BoxCollider triggerArea = default;

    // リスポーン位置
    [SerializeField]
    [Tooltip("リスポーン位置を設定する。")]
    Vector3 respawnPosition = Vector3.zero;

    // スポーン時間
    [SerializeField]
    [Tooltip("リスポーンの時間を設定する。")]
    float spawnTimeSecond = 0;

    [SerializeField]
    [Tooltip("このトリガーの発動回数を設定する。\nデスポーンからリスポーンまでで１回分。\n０以下の場合は無限。")]
    int activateCount = 0;

    // スポーンフラグ
    bool isSpawn = false;

    // スポーン時間
    float spawnTime = 0;

    // 現在の発動回数
    int currentActivateCount = 0;

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        if (isSpawn)
        {
            spawnTime += Time.deltaTime;
            if (spawnTime >= spawnTimeSecond)
            {
                // 消した鬼を表示
                targetCharacter?.gameObject.SetActive(true);
                // 表示した鬼をリスポーン位置までワープさせる
                targetCharacter?.Warp(transform.position + respawnPosition);

                // 発動回数をカウント
                currentActivateCount++;

                // フラグを倒す
                isSpawn = false;
                // 時間をリセット
                spawnTime = 0;
            }
        }

        // 発動回数を超えたら消す
        if (activateCount <= 0) { return; }
        if (currentActivateCount >= activateCount)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ギズモ
    /// </summary>
    void OnDrawGizmos()
    {
        // リスポーン位置に球体を表
        Gizmos.DrawSphere(transform.position + respawnPosition, 0.3f);
        // 線で結ぶ
        Gizmos.DrawLine(transform.position, transform.position + respawnPosition);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            targetCharacter.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーだったら
        if (other.tag == "Player")
        {
            if (!targetCharacter.gameObject.activeSelf)
            {
                isSpawn = true;
            }
        }
    }
}
