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
    CapsuleCollider playerCollider = default;               // プレイヤーのコライダー
    [SerializeField]
    PlayerBreathController breathController = default;      // 息管理クラス

    bool isSafetyArea = false;                              // 安全地帯内にいないかどうか
    bool isLookEnemy = false;                               // 敵が見えているかどうか
    List<GameObject> nearEnemy = new List<GameObject>();    // 安全距離内にいる敵のリスト
    List<bool> visibleEnemy = new List<bool>();             // 見えている敵がいるかのリスト
    public HeartSoundType HeartSound { get; private set; } = HeartSoundType.NORMAL;      // 心音

    /// <summary>
    /// トリガーがヒットしたら
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            nearEnemy.Add(other.gameObject);
        }
    }

    /// <summary>
    /// トリガーが離れたら
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            nearEnemy.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        isSafetyArea = false;
        isLookEnemy = false;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // ステートの変更
        ChangeHideState();

        // 心音の変更
        ChangeHeartSound();
    }

    /// <summary>
    /// ステートの変更
    /// </summary>
    void ChangeHideState()
    {
        if (nearEnemy.Count > 0)
        {
            isSafetyArea = true;
        }
        else
        {
            isSafetyArea = false;
        }
        if (visibleEnemy.Contains(true))
        {
            isLookEnemy = true;
        }
        else
        {
            isLookEnemy = false;
        }
        visibleEnemy.Clear();
    }

    /// <summary>
    /// 敵が見えているかどうか
    /// </summary>
    public void VisibleEnemy(Transform enemy, float height)
    {
        if (VisibleEnemyRay(enemy, height))
        {
            visibleEnemy.Add(true);
        }
        else
        {
            visibleEnemy.Add(false);
        }
    }

    /// <summary>
    /// 敵が見えているかどうかのRaycast
    /// </summary>
    bool VisibleEnemyRay(Transform enemy, float height)
    {
        // 敵のそれぞれの座標
        Vector3 enemyTop = new Vector3(enemy.position.x, enemy.position.y + (height / 2), enemy.position.z);
        Vector3 enemyMiddle = enemy.position;
        Vector3 enemyBottom = new Vector3(enemy.position.x, enemy.position.y - (height / 2) + 0.01f, enemy.position.z);

        // レイのスタート位置
        Vector3 start = new Vector3(transform.position.x, transform.position.y + (playerCollider.height / 2), transform.position.z);

        // レイの向き
        Vector3 dir = Vector3.zero;

        // レイの距離
        float distance = 0.0f;

        // レイヤーマスク(プレイヤーからレイが伸びているので除外)
        int layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Locker") | 1 << LayerMask.NameToLayer("Bed") | 1 << LayerMask.NameToLayer("SafetyArea");
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
        if (isSafetyArea)
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

        // 心音の変更に合わせた処理
        breathController.ChangeHideDecrement(HeartSound);
    }
}
