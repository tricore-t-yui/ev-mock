using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OniSpawnTrigger : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent oni = default;

    [SerializeField]
    BoxCollider triggerArea = default;

    // リスポーン位置
    [SerializeField]
    Vector3 respawnPosition = Vector3.zero;

    // スポーン時間
    [SerializeField]
    float spawnTimeSecond = 0;

    // スポーンフラグ
    bool isSpawn = false;

    // スポーン時間
    float spawnTime = 0;

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
                oni?.gameObject.SetActive(true);
                // 表示した鬼をリスポーン位置までワープさせる
                oni?.Warp(transform.position + respawnPosition);

                // フラグを倒す
                isSpawn = false;
                // 時間をリセット
                spawnTime = 0;
            }
        }
    }

    /// <summary>
    /// ギズモ
    /// </summary>
    void OnDrawGizmos()
    {
        // リスポーン位置に球体を表
        Gizmos.DrawSphere(transform.position + respawnPosition, 0.3f);
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーだったら
        if (other.tag == "Player")
        {
            isSpawn = true;
        }
    }
}
