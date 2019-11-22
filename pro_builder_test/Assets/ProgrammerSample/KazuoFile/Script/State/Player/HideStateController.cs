using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 隠れる状態管理クラス
/// </summary>
public class HideStateController : MonoBehaviour
{
    /// <summary>
    /// 心音
    /// </summary>
    public enum HeartSoundType
    {
        NORMAL,     // 通常
        MEDIUM,     // 中
        LARGE,      // 大
    }

    [SerializeField]
    CapsuleCollider playerCollider = default;           // プレイヤーのコライダー
    [SerializeField]
    PlayerBreathController breathController = default;  // 息管理クラス

    MeshRenderer mesh = default;                        // 敵のメッシュ
    CapsuleCollider enemyCollider = default;            // 敵のコライダー

    bool isSafety = false;                              // 安全地帯内にいないかどうか
    bool isLookEnemy = false;                           // 敵が見えているかどうか
    public HeartSoundType HeartSound { get; private set; } = HeartSoundType.NORMAL;      // 心音

    /// <summary>
    /// トリガーがヒットしたら
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            mesh = other.GetComponent<MeshRenderer>();
            enemyCollider = other.GetComponent<CapsuleCollider>();
            isSafety = false;
        }
    }

    /// <summary>
    /// トリガーがヒットしている間
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            if (mesh.isVisible && VisibleEnemyRay(other.gameObject.transform))
            {
                isLookEnemy = true;
            }
            else
            {
                isLookEnemy = false;
            }
        }
    }

    /// <summary>
    /// トリガーが離れたら
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            isSafety = true;
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 心音の変更
        ChangeHeartSound();

        // 心音の変更に合わせた処理
        breathController.ChangeHideDecrement(HeartSound);
    }

    /// <summary>
    /// 敵が見えているかどうかのRaycast
    /// </summary>
    bool VisibleEnemyRay(Transform enemy)
    {
        // 敵のそれぞれの座標
        Vector3 enemyTop = new Vector3(enemy.position.x, enemy.position.y + (enemyCollider.height / 2), enemy.position.z);
        Vector3 enemyMiddle = enemy.position;
        Vector3 enemyBottom = new Vector3(enemy.position.x, enemy.position.y - (enemyCollider.height / 2) + 0.01f, enemy.position.z);

        // レイのスタート位置
        Vector3 start = new Vector3(transform.position.x, transform.position.y + (playerCollider.height / 2), transform.position.z);

        // レイの向き
        Vector3 dir = Vector3.zero;

        // レイの距離
        float distance = 0.0f;

        // レイヤーマスク(プレイヤーからレイが伸びているので除外)
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        layerMask = ~layerMask;

        for (int i = 0; i < 3; i++)
        {
            // 処理回数によってレイの向きと距離を計算
            switch(i)
            {
                case 0: dir = (enemyTop - start).normalized;    distance = (start - enemyTop).magnitude; break;
                case 1: dir = (enemyMiddle - start).normalized; distance = (start - enemyMiddle).magnitude; break;
                case 2: dir = (enemyBottom - start).normalized; distance = (start - enemyBottom).magnitude; break;
            }

            // レイ作成
            Ray ray = new Ray(start, dir);
            RaycastHit hit = default;

            // デバック用ライン
            Debug.DrawLine(start, start + (dir * distance), Color.red);

            // レイに当たったらtrue、外れていたらfalse
            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 心音の変更
    /// </summary>
    void ChangeHeartSound()
    {
        if (!isSafety)
        {
            // 安全地帯内に敵がいて、まだ敵が見えていない状態(消費中)
            HeartSound = HeartSoundType.MEDIUM;

            if (isLookEnemy)
            {
                // 安全地帯内に敵がいて、敵が見えている状態(消費大)
                HeartSound = HeartSoundType.LARGE;
            }
        }
        else
        {
            // 安全地帯内に敵がおらず、敵が見えていない状態(消費小)
            HeartSound = HeartSoundType.NORMAL;

            // 安全地帯内に敵がおらず、敵が見えている状態
            if (isLookEnemy)
            {
                // 安全地帯内に敵がいて姿を見ている状態(消費中)
                HeartSound = HeartSoundType.MEDIUM;
            }
        }
    }
}
